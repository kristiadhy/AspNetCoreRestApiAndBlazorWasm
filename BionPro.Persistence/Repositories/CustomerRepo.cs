using Application.Repositories;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

namespace Persistence.Repositories;

public sealed class CustomerRepo : MethodBase<CustomerMD>, ICustomerRepo
{
    public CustomerRepo(AppDBContext dbContext) : base(dbContext) { }

    public async Task<PagedList<CustomerMD>> GetAll(CustomerParam customerParam, bool trackChanges)
    {
        var customers = await FindAll(trackChanges)
            .Sort(customerParam.OrderBy) //It's a local method
            .Skip((customerParam.PageNumber - 1) * customerParam.PageSize)
            .Take(customerParam.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges).CountAsync();

        return new PagedList<CustomerMD>(customers, count, customerParam.PageNumber, customerParam.PageSize);
    }

    public async Task<PagedList<CustomerMD>> GetByParameters(CustomerParam customerParam, bool trackChanges)
    {
        var customers = await FindAll(trackChanges)
            .SearchByName(customerParam.srcByName) //It's a local method
            .Sort(customerParam.OrderBy) //It's a local method
            .Skip((customerParam.PageNumber - 1) * customerParam.PageSize)
            .Take(customerParam.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges)
            .SearchByName(customerParam.srcByName)
            .CountAsync();

        return new PagedList<CustomerMD>(customers, count, customerParam.PageNumber, customerParam.PageSize);
    }

    public async Task<CustomerMD?> GetByID(Guid customerID, bool trackChanges)
    {
        var customer = await FindByCondition(x => x.CustomerID == customerID, trackChanges).FirstOrDefaultAsync();
        if (customer is not null)
            return customer;
        else
            return null;
    }

    public void CreateEntity(CustomerMD entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        Create(entity, cancellationToken);
    }

    public void UpdateEntity(CustomerMD entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        Update(entity, cancellationToken);
    }

    public void DeleteEntity(CustomerMD entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        Delete(entity, cancellationToken);
    }
}
