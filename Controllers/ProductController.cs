using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
  [Route("api/product")]
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

    [HttpGet("{name}")]
    public ActionResult GetProduct(string name)
    {
      var product = _productRepo
                      .Find(p => p.Name == name)
                      .Include(p => p.User)
                      .FirstOrDefault();

      var productResult = _mapper.Map<ProductReadDTO>(product);
      return Ok(productResult);
    }

    [HttpPost]
    public async Task<ActionResult> PostProduct(ProductCreateDTO product)
    {
      var newProduct = _mapper.Map<Product>(product);

      await _productRepo.CreateAsync(newProduct);

      var newProductRead = _mapper.Map<ProductReadDTO>(newProduct);

      return CreatedAtAction(nameof(GetProduct), new { name = newProduct.Name }, newProductRead);
    }
  }
}
