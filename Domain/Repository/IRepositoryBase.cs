using System;
using System.Linq;
using System.Linq.Expressions;

namespace ShoppingAPI.Domain.Repository
{
  public interface IRepositoryBase<T>
  {
    IQueryable<T> Find(Expression<Func<T, bool>> expression);

    void Create(T entity);

    void Update(T entity);
    
    void Delete(T entity);
  }
}
