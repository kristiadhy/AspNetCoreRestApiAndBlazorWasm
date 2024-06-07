using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Services.Contracts;

public interface ICustomerService : IServiceBase<CustomerDTO>
{
    Task<(IEnumerable<CustomerDTO> customerDTO, MetaData metaData)> GetByParametersAsync(Guid customerID, CustomerParam customerParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CustomerDTO> GetByCustomerIDAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default);

    Task<(CustomerDTO customerToPatch, CustomerModel customer)> GetCustomerForPatchAsync(Guid customerID, bool empTrackChanges, CancellationToken cancellationToken = default);
    Task SaveChangesForPatchAsync(CustomerDTO customerToPatch, CustomerModel customer, CancellationToken cancellationToken = default);
}