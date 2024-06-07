using Application.IRepositories;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

namespace Persistence.Repositories;

public sealed class SupplierRepo : MethodBase<SupplierModel>, ISupplierRepo
{
    public SupplierRepo(AppDBContext dbContext) : base(dbContext) { }

    public async Task<PagedList<SupplierModel>> GetAllAsync(SupplierParam supplierParam, bool trackChanges)
    {
        var customers = await FindAll(trackChanges)
            .Sort(supplierParam.OrderBy) //It's a local method
            .Skip((supplierParam.PageNumber - 1) * supplierParam.PageSize)
            .Take(supplierParam.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges).CountAsync();

        return new PagedList<SupplierModel>(customers, count, supplierParam.PageNumber, supplierParam.PageSize);
    }

    public async Task<PagedList<SupplierModel>> GetByParametersAsync(SupplierParam supplierParam, bool trackChanges)
    {
        var customers = await FindAll(trackChanges)
            .SearchByName(supplierParam.srcByName) //It's a local method
            .Sort(supplierParam.OrderBy) //It's a local method
            .Skip((supplierParam.PageNumber - 1) * supplierParam.PageSize)
            .Take(supplierParam.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges)
            .SearchByName(supplierParam.srcByName)
            .CountAsync();

        return new PagedList<SupplierModel>(customers, count, supplierParam.PageNumber, supplierParam.PageSize);
    }

    public async Task<SupplierModel?> GetByIDAsync(Guid customerID, bool trackChanges)
    {
        var customer = await FindByCondition(x => x.SupplierID == customerID, trackChanges).FirstOrDefaultAsync();
        if (customer is not null)
            return customer;
        else
            return null;
    }

    public void CreateEntity(SupplierModel entity, bool trackChanges)
    {
        Create(entity);
    }

    public void UpdateEntity(SupplierModel entity, bool trackChanges)
    {
        Update(entity);
    }

    public void DeleteEntity(SupplierModel entity, bool trackChanges)
    {
        Delete(entity);
    }
}
