

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingAPI.API.Controllers.Mappings;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.API.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/order")]
  public class OrderController : ControllerBase
  {
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(IOrderService orderService, IMapper mapper)
    {
      _orderService = orderService;
      _mapper = mapper;
    }

    [HttpGet("{orderId}")]
    public ActionResult GetOrderById(Guid orderId)
    {
      var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
      var order = _orderService
                      .Find(o => o.OrderId == orderId && o.UserId == userId)
                      .FirstOrDefault();

      if (order == null)
      {
        return NotFound();
      }

      var orderResult = _mapper.Map<OrderReadDTO>(order);
      return Ok(orderResult);
    }

    [HttpGet]
    public ActionResult GetOrders()
    {
      var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
      var orders = _orderService
                      .Find(o => o.UserId == userId)
                      .ToList();

      var ordersResult = _mapper.Map<IEnumerable<OrderReadDTO>>(orders);
      return Ok(ordersResult);
    }

    [HttpPost("{orderId}")]
    public ActionResult AddRemoveProduct(Guid orderId, ProductQuantity productQuantity)
    {
      var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
      var order = _orderService
                      .Find(o => o.OrderId == orderId && o.UserId == userId)
                      .FirstOrDefault();

      if (order == null)
      {
        return NotFound();
      }
      
      order = _orderService.AddRemoveProductsInOrder(orderId, new List<ProductQuantity> { productQuantity });

      var orderReadDTO = _mapper.Map<OrderReadDTO>(order);

      return CreatedAtAction(nameof(GetOrderById), new { orderId = orderReadDTO.OrderId }, orderReadDTO);
    }
  }
}
