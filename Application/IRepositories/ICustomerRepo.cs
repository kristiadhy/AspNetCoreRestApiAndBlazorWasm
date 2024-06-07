using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;

public interface ICustomerRepo : IRepositoryBase<CustomerModel>
{
    Task<PagedList<CustomerModel>> GetAllAsync(CustomerParam customerParam, bool trackChanges);
    Task<PagedList<CustomerModel>> GetByParametersAsync(CustomerParam customerParam, bool trackChanges);
    Task<CustomerModel?> GetByIDAsync(Guid customerID, bool trackChanges);
}
