namespace Application.IRepositories;
public interface IRepositoryBase<T>
{
    void CreateEntity(T entity, bool trackChanges, CancellationToken cancellationToken = default);
    void UpdateEntity(T entity, bool trackChanges, CancellationToken cancellationToken = default);
    void DeleteEntity(T entity, bool trackChanges, CancellationToken cancellationToken = default);
}
