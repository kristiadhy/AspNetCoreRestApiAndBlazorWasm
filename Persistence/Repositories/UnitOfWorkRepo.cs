using Application.IRepositories;
using Persistence.Context;

namespace Persistence.Repositories;

public sealed class UnitOfWorkRepo : IUnitOfWorkRepo
{
    private readonly AppDBContext _dbContext;

    public UnitOfWorkRepo(AppDBContext dbContext) => _dbContext = dbContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _dbContext.SaveChangesAsync(cancellationToken);
}