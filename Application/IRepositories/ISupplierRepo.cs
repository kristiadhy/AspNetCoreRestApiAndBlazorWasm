using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;
public interface ISupplierRepo : IRepositoryBase<SupplierModel>
{
    Task<PagedList<SupplierModel>> GetAllAsync(SupplierParam supplierParam, bool trackChanges);
    Task<PagedList<SupplierModel>> GetByParametersAsync(SupplierParam supplierParam, bool trackChanges);
    Task<SupplierModel?> GetByIDAsync(Guid customerID, bool trackChanges);
}
