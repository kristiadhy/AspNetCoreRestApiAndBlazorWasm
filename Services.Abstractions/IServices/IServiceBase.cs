namespace Services.Contracts;
public interface IServiceBase<T>
{
    Task<T> CreateAsync(T entity, bool trackChanges, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, bool trackChanges, CancellationToken cancellationToken = default);
}
