using Application.IRepositories;
using Domain.Entities;
using Domain.Parameters;

namespace Application.Repositories;

public interface ICustomerRepo : IRepositoryBase<CustomerMD>
{
    Task<PagedList<CustomerMD>> GetAllAsync(CustomerParam customerParam, bool trackChanges);
    Task<PagedList<CustomerMD>> GetByParametersAsync(CustomerParam customerParam, bool trackChanges);
    Task<CustomerMD?> GetByIDAsync(Guid customerID, bool trackChanges);
}
