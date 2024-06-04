namespace Application.IRepositories;
public interface IRepositoryBase<T>
{
    void CreateEntity(T entity, bool trackChanges);
    void UpdateEntity(T entity, bool trackChanges);
    void DeleteEntity(T entity, bool trackChanges);
}
