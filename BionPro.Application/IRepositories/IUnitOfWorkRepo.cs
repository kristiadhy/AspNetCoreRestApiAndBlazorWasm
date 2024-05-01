namespace Application.Repositories;

public interface IUnitOfWorkRepo
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}