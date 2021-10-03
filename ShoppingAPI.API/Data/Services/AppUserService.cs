using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.API.Data.Services.Exceptions;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;
using ShoppingAPI.Domain.Exceptions;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.API.Data.Services
{
  public class AppUserService : ServiceBase<AppUser>, IAppUserService
  {
    private readonly ApplicationContext _appContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;

    public AppUserService(ApplicationContext appContext, 
                          UserManager<AppUser> userManager, 
                          IOrderService orderRepo,
                          IProductService productService) : base(appContext)
    {
      _appContext = appContext;
      _userManager = userManager;
      _orderService = orderRepo;
      _productService = productService;
    }

    public Order CheckoutCart(string userId)
    {
      var user = _userManager.Users
                      .Where(u => u.Id == userId)
                      .Include(u => u.CartProducts)
                      .ThenInclude(u => u.Product)
                      .FirstOrDefault();
      if (user == null)
      {
        throw new DoesNotExist<AppUser>(new List<string> { userId });
      }
      if (user.CartProducts.Count() == 0)
      {
        throw new EmptyCart();
      }

      var productQuantities = user.CartProducts.Select(cp => new ProductQuantity(cp.Product.ProductId, cp.Quantity));
      var newOrder = _orderService.CreateOrder(userId, productQuantities);

      // Clear cart
      ClearCart(user.Id);
      return newOrder;
    }

    public void ClearCart(string userId)
    {
      var user = _userManager.Users
                      .Where(u => u.Id == userId)
                      .Include(u => u.CartProducts)
                      .FirstOrDefault();
      _appContext.Cart.RemoveRange(user.CartProducts);
      _appContext.SaveChanges();
    }

    public IEnumerable<Cart> AddRemoveProductInCart(string userId, Guid productId, float quantity)
    {
      var user = _userManager.Users
                  .Where(u => u.Id == userId)
                  .Include(u => u.CartProducts)
                  .ThenInclude(cp => cp.Product)
                  .FirstOrDefault();

      if (user == null)
      {
        throw new DoesNotExist<AppUser>(new List<string> { userId });
      }

      var cartProducts = user.CartProducts;

      var product = _appContext.Products
                      .Where(p => p.ProductId == productId)
                      .FirstOrDefault();

      if (product == null)
      {
        throw new DoesNotExist<Product>(new List<Guid> { productId });
      }

      // Add/Remove cart product or update existing one
      var cartProduct = cartProducts
                          .Where(cp => cp.Product.ProductId == productId)
                          .FirstOrDefault();

      if (cartProduct != null)
      {
        try
        {
          cartProduct.Quantity += quantity;

          if (cartProduct.Quantity > product.Quantity)
          {
            throw new NotEnoughProductsInStock(new ProductQuantity(product.ProductId, quantity));
          }
        }
        catch (DomainException e)
        {
          if (e.DomainExceptionType == DomainExceptionTypes.CartProductInvalidQuantity)
          {
            _appContext.Remove(cartProduct);
          }
        }
      }
      else
      {
        if (quantity > product.Quantity)
        {
          throw new NotEnoughProductsInStock(new ProductQuantity(product.ProductId, quantity));
        }

        if (quantity > 0)
        {
          var newCartProduct = new Cart(user.Id, product.Id, quantity);
          cartProducts.Add(newCartProduct);
          cartProduct = newCartProduct;
        }
      }

      // Save
      _appContext.SaveChanges();
      return cartProducts;
    }
  }
}
