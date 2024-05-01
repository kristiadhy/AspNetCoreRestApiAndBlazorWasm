using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public abstract class MethodBase<T> : IMethodBase<T> where T : class
{
    protected AppDBContext appDBContext;

    public MethodBase(AppDBContext repositoryContext) => appDBContext = repositoryContext;

    public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? appDBContext.Set<T>().AsNoTracking() : appDBContext.Set<T>();
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) => !trackChanges ? appDBContext.Set<T>().Where(expression).AsNoTracking() : appDBContext.Set<T>().Where(expression);
    public void Create(T entity, CancellationToken cancellationToken) => appDBContext.Set<T>().Add(entity);
    public void Update(T entity, CancellationToken cancellationToken) => appDBContext.Set<T>().Update(entity);
    public void Delete(T entity, CancellationToken cancellationToken) => appDBContext.Set<T>().Remove(entity);
}