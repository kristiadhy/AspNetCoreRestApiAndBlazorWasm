using Application.Repositories;
using Persistence.Context;

namespace Persistence.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IUnitOfWorkRepo> _lazyUnitOfWorkRepo;
    private readonly Lazy<ICustomerRepo> _lazyCustomerRepo;

    public RepositoryManager(AppDBContext dbContext)
    {
        _lazyCustomerRepo = new Lazy<ICustomerRepo>(() => new CustomerRepo(dbContext));
        _lazyUnitOfWorkRepo = new Lazy<IUnitOfWorkRepo>(() => new UnitOfWorkRepo(dbContext));
    }

    public ICustomerRepo CustomerRepo => _lazyCustomerRepo.Value;
    public IUnitOfWorkRepo UnitOfWorkRepo => _lazyUnitOfWorkRepo.Value;
}
