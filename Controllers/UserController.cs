using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
  public class UserController : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public UserController(UserManager<AppUser> userManager,
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
  }
}
