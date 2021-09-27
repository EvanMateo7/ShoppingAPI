

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingAPI.API.Controllers.Mappings;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.API.Controllers
{
  [ApiController]
  [Route("api/order")]
  public class OrderController : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IOrderRepository _orderRepo;
    private readonly IMapper _mapper;

    public OrderController(UserManager<AppUser> userManager,
                                IOrderRepository orderRepo,
                                IMapper mapper)
    {
      _userManager = userManager;
      _orderRepo = orderRepo;
      _mapper = mapper;
    }

    [HttpGet("{id}")]
    public ActionResult GetOrderById(Guid id)
    {
      var order = _orderRepo
                      .Find(o => o.OrderId == id)
                      .FirstOrDefault();

      if (order == null)
      {
        return NotFound();
      }

      var orderResult = _mapper.Map<OrderReadDTO>(order);
      return Ok(orderResult);
    }

    // TODO: Use Authorization instead of userId
    [HttpGet]
    public ActionResult GetOrders(string userId)
    {
      var orders = _orderRepo
                      .Find(o => o.UserId == userId)
                      .ToList();

      var ordersResult = _mapper.Map<IEnumerable<OrderReadDTO>>(orders);
      return Ok(ordersResult);
    }

    [HttpPost("{orderId}")]
    public ActionResult AddRemoveProduct(Guid orderId, ProductQuantity productQuantity)
    {
      var order = _orderRepo.AddRemoveProductInOrder(orderId, new List<ProductQuantity> { productQuantity });

      var orderReadDTO = _mapper.Map<OrderReadDTO>(order);

      return CreatedAtAction(nameof(GetOrderById), new { id = orderReadDTO.OrderId }, orderReadDTO);
    }
  }
}
