using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingAPI.Domain.Repository;

namespace ShoppingAPI.Data.Repositories
{
  public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
  {
    private readonly ApplicationContext _appContext;

    public RepositoryBase(ApplicationContext appContext)
    {
      _appContext = appContext;
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> expression)
    {
      return _appContext.Set<T>().Where(expression).AsNoTracking();
    }

    public async Task<Task> CreateAsync(T entity)
    {
      await _appContext.Set<T>().AddAsync(entity);
      return _appContext.SaveChangesAsync();
    }

    public void Update(T entity)
    {
      _appContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
      _appContext.Set<T>().Remove(entity);
    }
  }
}
