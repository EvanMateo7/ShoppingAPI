

using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Data.Mappings;
using ShoppingAPI.Data.Util;
using ShoppingAPI.Domain;
using ShoppingAPI.Domain.Repository;

namespace ShoppingAPI.Controllers
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

    [HttpPost]
    public ActionResult CreateOrder(OrderCreateDTO orderCreate)
    {
      var orderCreated = _orderRepo.Create(orderCreate.ProductIDs);

      var orderReadDTO = _mapper.Map<OrderReadDTO>(orderCreated);

      return CreatedAtAction(nameof(GetOrders), new { userId = orderCreated.UserId }, orderReadDTO);
    }
  }
}