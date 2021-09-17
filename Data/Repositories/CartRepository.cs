using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Data.Util;
using ShoppingAPI.Domain;
using ShoppingAPI.Domain.Repository;

namespace ShoppingAPI.Data.Repositories
{
  public class CartRepository : RepositoryBase<Product>, ICartRepository
  {
    private readonly ApplicationContext _appContext;

    public CartRepository(ApplicationContext appContext) : base(appContext)
    {
      _appContext = appContext;
    }
  }
}
