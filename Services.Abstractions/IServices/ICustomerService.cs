using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Services.Contracts;

public interface ICustomerService : IServiceBase<CustomerDTO>
{
    Task<(IEnumerable<CustomerDTO> customerDTO, MetaData metaData)> GetByParametersAsync(Guid customerID, CustomerParam customerParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CustomerDTO> GetByCustomerIDAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default);

    Task<(CustomerDTO customerToPatch, CustomerMD customer)> GetCustomerForPatchAsync(Guid customerID, bool empTrackChanges, CancellationToken cancellationToken = default);
    Task SaveChangesForPatchAsync(CustomerDTO customerToPatch, CustomerMD customer, CancellationToken cancellationToken = default);
}