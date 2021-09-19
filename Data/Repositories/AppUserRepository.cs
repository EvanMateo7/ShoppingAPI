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
  public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
  {
    private readonly ApplicationContext _appContext;

    public AppUserRepository(ApplicationContext appContext) : base(appContext)
    {
      _appContext = appContext;
    }
  }
}
