using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Services.Contracts;

public interface ISupplierService : IServiceBase<SupplierDto>
{
    Task<(IEnumerable<SupplierDto> supplierDto, MetaData metaData)> GetByParametersAsync(Guid supplierID, SupplierParam supplierParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<SupplierDto> GetBySupplierIDAsync(Guid supplierID, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid supplierID, bool trackChanges, CancellationToken cancellationToken = default);

    Task<(SupplierDto supplierToPatch, SupplierModel supplier)> GetSupplierForPatchAsync(Guid supplierID, bool empTrackChanges, CancellationToken cancellationToken = default);
    Task SaveChangesForPatchAsync(SupplierDto supplierToPatch, SupplierModel supplier, CancellationToken cancellationToken = default);
}