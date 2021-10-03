using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.API.Controllers.Mappings;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;

namespace ShoppingAPI.API.Controllers
{
  [ApiController]
  [Route("api/user")]
  public class AppUserController : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppUserService _appUserService;
    private readonly IMapper _mapper;

    public AppUserController(UserManager<AppUser> userManager,
                                IAppUserService appUserRepo,
                                IMapper mapper)
    {
      _userManager = userManager;
      _appUserService = appUserRepo;
      _mapper = mapper;
    }

    [HttpGet("{username}")]
    public ActionResult GetUser(string username)
    {
      var user = _userManager
                      .Users
                      .Where(u => u.UserName == username)
                      .FirstOrDefault();

      if (user == null)
      {
        return NotFound();
      }

      var userResult = _mapper.Map<AppUserReadDTO>(user);
      return Ok(userResult);
    }

    [Authorize]
    [HttpGet("cart")]
    public ActionResult GetUserCart()
    {
      var username = User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
      var user = _userManager.Users
                      .Where(u => u.UserName == username)
                      .Include(u => u.CartProducts)
                      .ThenInclude(cp => cp.Product)
                      .FirstOrDefault();

      if (user == null)
      {
        return NotFound();
      }

      var cartProducts = user.CartProducts.Select(cp => cp.Product);
      var cartResult = _mapper.Map<IEnumerable<ProductReadDTO>>(cartProducts);

      return Ok(cartResult);
    }

    [Authorize]
    [HttpPost("cart")]
    public ActionResult AddRemoveProduct(OrderProductCreateDTO orderProductCreate)
    {
      var username = User.FindFirst(c => c.Type == ClaimTypes.Email).Value;
      var user = _userManager.Users
                      .Where(u => u.UserName == username)
                      .Include(u => u.CartProducts)
                      .ThenInclude(cp => cp.Product)
                      .FirstOrDefault();

      IEnumerable<Cart> cart = _appUserService.AddRemoveProductInCart(user.Id,
                                            orderProductCreate.ProductId,
                                            orderProductCreate.Quantity);

      var cartProducts = user.CartProducts.Select(cp => cp.Product);
      var cartResult = _mapper.Map<IEnumerable<ProductReadDTO>>(cartProducts);
      return CreatedAtAction(nameof(GetUserCart), cartResult);
    }

    [Authorize]
    [HttpPost("cart/checkout")]
    public ActionResult Checkout()
    {
      var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

      Order orderCreated = _appUserService.CheckoutCart(userId);

      var orderReadDTO = _mapper.Map<OrderReadDTO>(orderCreated);

      return CreatedAtAction(nameof(OrderController.GetOrderById),
                              nameof(OrderController).Replace(nameof(Controller), string.Empty),
                              new { orderId = orderReadDTO.OrderId },
                              orderReadDTO);
    }
  }
}
