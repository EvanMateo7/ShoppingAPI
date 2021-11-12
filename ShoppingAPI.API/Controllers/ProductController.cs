using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.API.Controllers.Mappings;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;
using ShoppingAPI.Domain.ValueObjects;

namespace ShoppingAPI.API.Controllers
{
  [ApiController]
  [Route("api/product")]
  public class ProductController : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(UserManager<AppUser> userManager,
                                IProductService productService,
                                IMapper mapper)
    {
      _userManager = userManager;
      _productService = productService;
      _mapper = mapper;
    }

    [HttpGet("{id}")]
    public ActionResult GetProductById(Guid id)
    {
      var product = _productService
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

    [HttpGet]
    public ActionResult GetProducts([FromQuery] SearchPaginationQuery pageQuery)
    {
      var product = _productService
                      .QueryAll(pageQuery)
                      .Include(p => p.User)
                      .ToList();

      var productResult = _mapper.Map<IEnumerable<ProductReadDTO>>(product);
      return Ok(productResult);
    }

    [HttpPost]
    public ActionResult CreateProduct(ProductCreateDTO product)
    {
      var newProduct = _mapper.Map<Product>(product);

      try
      {
        var num = _productService.Create(newProduct);
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
      var targetProduct = _productService.Find(p => p.ProductId == productId).FirstOrDefault();

      if (targetProduct == null)
      {
        return NotFound();
      }

      // Create a ProductCreateDTO from target product
      var productToPatch = _mapper.Map<ProductCreateDTO>(targetProduct);

      // Apply patch
      productPatchDoc.ApplyTo(productToPatch, ModelState);
      if (!TryValidateModel(productToPatch))
      {
        return ValidationProblem(ModelState);
      }

      // Update product
      _mapper.Map(productToPatch, targetProduct);
      _productService.Update(targetProduct);

      var patchedTargetProduct = _mapper.Map<ProductReadDTO>(targetProduct);
      return CreatedAtAction(nameof(GetProductById), new { name = patchedTargetProduct.Name }, patchedTargetProduct);
    }

    [HttpDelete]
    public ActionResult DeleteProduct(Guid productId)
    {
      var targetProduct = _productService.Find(p => p.ProductId == productId).FirstOrDefault();

      if (targetProduct == null)
      {
        return NotFound();
      }

      _productService.Delete(targetProduct);

      return NoContent();
    }
  }
}
