using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Data.Mappings;
using ShoppingAPI.Data.Util;
using ShoppingAPI.Domain;
using ShoppingAPI.Domain.Repository;

namespace ShoppingAPI.Controllers
{
  [ApiController]
  [Route("api/user")]
  public class AppUserController : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public AppUserController(UserManager<AppUser> userManager,
                                IProductRepository productRepo,
                                IMapper mapper)
    {
      _userManager = userManager;
      _productRepo = productRepo;
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

      var productResult = _mapper.Map<IEnumerable<ProductReadDTO>>(cartProducts);
      return Ok(productResult);
    }
  }
}
