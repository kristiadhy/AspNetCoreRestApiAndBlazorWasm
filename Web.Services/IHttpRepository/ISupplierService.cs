using Domain.DTO;
using Domain.Parameters;
using Features;

namespace Services.IRepositories;

public interface ISupplierService
{
    public Task<PagingResponse<SupplierDto>> GetSuppliers(SupplierParam supplierParam);
    public Task<SupplierDto> GetSupplierByID(Guid supplierID);
    public Task<HttpResponseMessage> Create(SupplierDto supplierDto);
    public Task<HttpResponseMessage> Update(SupplierDto supplierDto);
    public Task<HttpResponseMessage> Delete(Guid supplierID);
}
