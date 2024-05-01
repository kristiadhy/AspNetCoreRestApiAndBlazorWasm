namespace Services.Contracts;
public interface IServiceBase<T>
{
    Task<T> Create(T entity, bool trackChanges, CancellationToken cancellationToken = default);
    Task Update(T entity, bool trackChanges, CancellationToken cancellationToken = default);
}
