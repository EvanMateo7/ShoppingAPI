
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.API.Data;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;
using ShoppingAPI.API.Data.Repositories;

namespace ShoppingAPI.Tests
{
  public class TestFixture
  {
    public ApplicationContext Context { get; init; }
    public ProductRepository ProductRepo { get; init; }

    public IEnumerable<AppUser> Users { get; private set; }
    public IEnumerable<Product> Products { get; private set; }

    public TestFixture()
    {
      /*
        Warning:
          InMemoryDatabase does not enforce constaints like foreign key constraint.
      */
      var options = new DbContextOptionsBuilder<ApplicationContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

      Context = new ApplicationContext(options);

      ProductRepo = new ProductRepository(Context);

      Seed(Context);
    }

    private void Seed(ApplicationContext context)
    {
      // Users
      Users = new[]
      {
        new AppUser() { Id = Guid.NewGuid().ToString(), UserName = "test@test.com", Email = "test@test.com" }
      };

      foreach (var user in Users)
      {
        var userStore = new UserStore<AppUser>(context);
        var result = userStore.CreateAsync(user);
      }

      // Products
      var userId = Users.First().Id;
      Products = new[]
      {
        new Product() { UserId = userId, Name = "item1", Quantity = 10, Price = 100 },
        new Product() { UserId = userId, Name = "item2", Quantity = 20, Price = 200 },
        new Product() { UserId = userId, Name = "item3", Quantity = 30, Price = 300 }
      };

      context.Products.AddRange(Products);

      context.SaveChanges();
    }
  }
}
