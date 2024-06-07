namespace Application.IRepositories;

public interface IUnitOfWorkRepo
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}