using System;
using System.Linq;
using System.Linq.Expressions;

namespace ShoppingAPI.Domain.AggregateRoots
{
  public interface IRepositoryBase<T> where T : IAggregateRoot
  {
    IQueryable<T> Find(Expression<Func<T, bool>> expression);

    int Create(T entity);

    void Update(T entity);

    void Delete(T entity);
  }
}
