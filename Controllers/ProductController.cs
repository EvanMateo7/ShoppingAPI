using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Data.Mappings;
using ShoppingAPI.Domain;
using ShoppingAPI.Domain.Repository;

namespace ShoppingAPI.Controllers
{
  [ApiController]
  [Route("api")]
  public class ProductController : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public ProductController(UserManager<AppUser> userManager,
                                IProductRepository productRepo,
                                IMapper mapper)
    {
      _userManager = userManager;
      _productRepo = productRepo;
      _mapper = mapper;
    }

    [HttpGet("products")]
    public ActionResult GetProducts()
    {
      var products = _productRepo
                      .Find(p => p.Name == "chair")
                      .Include(p => p.User)
                      .ToList();
      var productsResults = _mapper.Map<IEnumerable<ProductReadDTO>>(products);
      return Ok(productsResults);
    }
  }
}
