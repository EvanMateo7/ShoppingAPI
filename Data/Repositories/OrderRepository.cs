using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Data.Repositories.Exceptions;
using ShoppingAPI.Data.Repositories.Records;
using ShoppingAPI.Domain;
using ShoppingAPI.Domain.Exceptions;
using ShoppingAPI.Domain.Repository;
using static ShoppingAPI.Data.Repositories.Exceptions.NotEnoughProductsInStock;

namespace ShoppingAPI.Data.Repositories
{
  public class OrderRepository : RepositoryBase<Order>, IOrderRepository
  {
    private readonly ApplicationContext _appContext;

    public OrderRepository(ApplicationContext appContext) : base(appContext)
    {
      _appContext = appContext;
    }

    public Order Create(AppUser user)
    {
      var productQuantities = user.CartProducts.Select(cp => new ProductQuantity(cp.Product.ProductId, cp.Quantity));

      var newOrder = new Order() { UserId = user.Id };
      _appContext.Database.CreateExecutionStrategy().ExecuteInTransaction(() =>
      {
        _appContext.Orders.Add(newOrder);
        _appContext.SaveChanges();

        // Add cart products to new order
        newOrder = AddRemoveProductInOrder(newOrder, productQuantities);

        // Clear cart after creating order
        _appContext.Cart.RemoveRange(user.CartProducts);
        
        _appContext.SaveChanges();

      }, () => true);

      return newOrder;
    }

    public Order AddRemoveProductInOrder(Order order, IEnumerable<ProductQuantity> productQuantities)
    {
      bool saveFailed;
      do
      {
        saveFailed = false;
        try
        {
          // Order
          if (order == null)
          {
            throw new DoesNotExist<Order>(new List<Guid> { order.OrderId });
          }

          // Products
          var productIds = productQuantities.Select(pq => pq.ProductId);

          var products = _appContext.Products.Where(p => productIds.Contains(p.ProductId));

          var ids = products.Select(p => p.ProductId);
          var nonExistingProducts = productIds.Except(ids);
          if (nonExistingProducts.Count() > 0)
          {
            throw new DoesNotExist<Product>(nonExistingProducts);
          }

          foreach(var product in products)
          {
            // Validate product quantity change
            var productsNotEnoughStock = new List<ProductQuantity>();
            var quantity = productQuantities.Where(pq => pq.ProductId == product.ProductId).Select(pq => pq.Quantity).Single();

            try
            {
              product.Quantity -= quantity;
            }
            catch (DomainException e)
            {
              if (e.DomainExceptionType == DomainExceptionTypes.ProductNegativeQuantity)
              {
                productsNotEnoughStock.Add(new ProductQuantity(product.ProductId, quantity));
              }
            }

            if (productsNotEnoughStock.Count() > 0)
            {
              throw new NotEnoughProductsInStock(new ProductQuantity(product.ProductId,quantity));
            }

            // Add/Remove order product or update existing one
            var orderProduct = _appContext.OrderProducts
                                .Where(op => op.OrderId == order.Id && op.ProductId == product.Id)
                                .FirstOrDefault();

            if (orderProduct != null)
            {
              try
              {
                orderProduct.Quantity += quantity;
              }
              catch (DomainException e)
              {
                if (e.DomainExceptionType == DomainExceptionTypes.OrderProductInvalidQuantity)
                {
                  _appContext.Remove(orderProduct);
                }
              }
            }
            else
            {
              if (quantity > 0)
              {
                var newOrderProduct = new OrderProduct(order.Id, product, quantity);
                order.OrderProducts.Add(newOrderProduct);
              }
            }
          }

          // Save
          _appContext.SaveChanges();
          return order;
        }
        catch (DbUpdateConcurrencyException e)
        {
          saveFailed = true;

          // Reload product entity
          e.Entries.Single().Reload();
        }
      } while (saveFailed);

      return null;
    }
  }
}
