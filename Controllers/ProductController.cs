using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpGet("{id}")]
    public ActionResult GetProductById(Guid id)
    {
      var product = _productRepo
                      .Find(p => p.ProductId == id)
                      .Include(p => p.User)
                      .FirstOrDefault();

      if (product == null)
      {
        return NotFound();
      }

      var productResult = _mapper.Map<ProductReadDTO>(product);
      return Ok(productResult);
    }

    [HttpPost]
    public ActionResult CreateProduct(ProductCreateDTO product)
    {
      var newProduct = _mapper.Map<Product>(product);

      try
      {
        var num = _productRepo.Create(newProduct);   
      }
      catch (DbUpdateException)
      {
        return Problem();
      }

      var newProductRead = _mapper.Map<ProductReadDTO>(newProduct);

      return CreatedAtAction(nameof(GetProductById), new { id = newProduct.ProductId }, newProductRead);
    }

    [HttpPatch("{productId}")]
    public ActionResult UpdateProduct(Guid productId, JsonPatchDocument<ProductCreateDTO> productPatchDoc)
    {
      var targetProduct = _productRepo.Find(p => p.ProductId == productId).FirstOrDefault();

      if (targetProduct == null)
      {
        return NotFound();
      }

      // Create a ProductCreateDTO from target product
      var productToPatch = _mapper.Map<ProductCreateDTO>(targetProduct);

      // Apply patch
      productPatchDoc.ApplyTo(productToPatch, ModelState);
      if(!TryValidateModel(productToPatch))
      {
          return ValidationProblem(ModelState);
      }

      // Update product
      _mapper.Map(productToPatch, targetProduct);
      _productRepo.Update(targetProduct);

      var patchedTargetProduct = _mapper.Map<ProductReadDTO>(targetProduct);
      return CreatedAtAction(nameof(GetProductById), new { name = patchedTargetProduct.Name }, patchedTargetProduct);
    }

    [HttpDelete]
    public ActionResult DeleteProduct(Guid productId)
    {
      var targetProduct = _productRepo.Find(p => p.ProductId == productId).FirstOrDefault();

      if (targetProduct == null)
      {
        return NotFound();
      }

      _productRepo.Delete(targetProduct);

      return NoContent();
    }
  }
}
