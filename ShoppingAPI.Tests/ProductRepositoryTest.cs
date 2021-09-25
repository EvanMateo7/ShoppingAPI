using System.Linq;
using ShoppingAPI.Domain;
using Xunit;
using Xunit.Abstractions;

namespace ShoppingAPI.Tests
{
  public class ProductRepositoryTest : IClassFixture<TestFixture>
  {
    private readonly ITestOutputHelper _output;
    private readonly TestFixture _fixture;

    public ProductRepositoryTest(ITestOutputHelper output, TestFixture fixture)
    {
      _output = output;
      _fixture = fixture;
    }

    [Fact]
    public void Find()
    {
      var firstProduct = _fixture.Products.First();
      var product = _fixture.ProductRepo.Find(p => p.Name == firstProduct.Name).FirstOrDefault();

      Assert.Equal(product.ProductId, firstProduct.ProductId);
    }

    [Fact]
    public void Create()
    {
      var newProduct = new Product() { Name = "ass", Quantity = 1, Price = 2 };
      _fixture.ProductRepo.Create(newProduct);
      var queryProduct = _fixture.Context.Products.Where(p => p.Name == newProduct.Name).FirstOrDefault();

      Assert.NotNull(queryProduct);
      Assert.Equal(newProduct.Name, queryProduct.Name);
    }

    [Fact]
    public void Update()
    {
      var firstProduct = _fixture.Products.First();
      var firstProductQuery = _fixture.ProductRepo.Find(p => p.Name == firstProduct.Name);
      var product = firstProductQuery.FirstOrDefault();

      Assert.Equal(product.ProductId, firstProduct.ProductId);

      var newProductName = "newname";
      firstProduct.Name = newProductName;
      _fixture.ProductRepo.Update(firstProduct);
      var updatedProduct = firstProductQuery.FirstOrDefault();

      Assert.Equal(newProductName, updatedProduct.Name);
    }

    [Fact]
    public void Delete()
    {
      var firstProduct = _fixture.Products.First();
      var firstProductQuery = _fixture.ProductRepo.Find(p => p.Name == firstProduct.Name);
      var product = firstProductQuery.FirstOrDefault();

      Assert.Equal(product.ProductId, firstProduct.ProductId);

      _fixture.ProductRepo.Delete(firstProduct);
      var updatedProduct = firstProductQuery.FirstOrDefault();

      Assert.Null(updatedProduct);
    }
  }
}
