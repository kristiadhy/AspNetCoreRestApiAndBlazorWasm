using Application.IRepositories;
using Domain.Entities;
using Domain.Parameters;

namespace Application.Repositories;

public interface ICustomerRepo : IRepositoryBase<CustomerMD>
{
    Task<PagedList<CustomerMD>> GetAll(CustomerParam customerParam, bool trackChanges);
    Task<PagedList<CustomerMD>> GetByParameters(CustomerParam customerParam, bool trackChanges);
    Task<CustomerMD?> GetByID(Guid customerID, bool trackChanges);
}
