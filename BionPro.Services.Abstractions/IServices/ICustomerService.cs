using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Services.Contracts;

public interface ICustomerService : IServiceBase<CustomerDTO>
{
    Task<(IEnumerable<CustomerDTO> customerDTO, MetaData metaData)> GetByParameters(Guid customerID, CustomerParam customerParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerDTO>> GetByCustomerID(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default);
    Task Delete(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default);

    Task<(CustomerDTO customerToPatch, CustomerMD customer)> GetCustomerForPatch(Guid customerID, bool empTrackChanges, CancellationToken cancellationToken = default);
    Task SaveChangesForPatch(CustomerDTO customerToPatch, CustomerMD customer, CancellationToken cancellationToken = default);
}