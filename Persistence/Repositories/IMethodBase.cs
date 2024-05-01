using System.Linq.Expressions;

namespace Persistence.Repositories;

public interface IMethodBase<T1>
{
    IQueryable<T1> FindAll(bool trackChanges);
    IQueryable<T1> FindByCondition(Expression<Func<T1, bool>> expression, bool trackChanges);
    void Create(T1 entity, CancellationToken cancellationToken);
    void Update(T1 entity, CancellationToken cancellationToken);
    void Delete(T1 entity, CancellationToken cancellationToken);
}