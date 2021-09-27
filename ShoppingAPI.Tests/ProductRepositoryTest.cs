using System.Linq;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;
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

      // Fixture is shared between all tests
      _fixture = fixture;

      // Re-initialize fixture for each test
      _fixture.Initialize();
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
      var firstProductQuery = _fixture.Context.Products.Where(p => p.Name == firstProduct.Name);
      var product = _fixture.Context.Products.Where(p => p.Name == firstProduct.Name).FirstOrDefault();

      Assert.Equal(product.ProductId, firstProduct.ProductId);

      var newProductName = "newname";
      product.Name = newProductName;
      _fixture.ProductRepo.Update(product);
      _fixture.Context.Entry(product).Reload();

      Assert.Equal(newProductName, product.Name);
    }

    [Fact]
    public void Delete()
    {
      var firstProduct = _fixture.Products.First();
      var firstProductQuery = _fixture.Context.Products.Where(p => p.Name == firstProduct.Name);
      var product = firstProductQuery.FirstOrDefault();

      Assert.Equal(product.ProductId, firstProduct.ProductId);

      _fixture.ProductRepo.Delete(firstProduct);
      var updatedProduct = firstProductQuery.FirstOrDefault();

      Assert.Null(updatedProduct);
    }
  }
}
