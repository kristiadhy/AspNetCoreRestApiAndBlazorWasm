using Domain.DTO;
using Domain.Parameters;
using Features;

namespace Services.IRepositories;

public interface ICustomerService
{
    public Task<PagingResponse<CustomerDTO>> GetCustomers(CustomerParam customerParameter);
    public Task<CustomerDTO> GetCustomerByID(Guid customerID);
    public Task<HttpResponseMessage> Create(CustomerDTO customerDTO);
    public Task<HttpResponseMessage> Update(CustomerDTO customerDTO);
    public Task<HttpResponseMessage> Delete(Guid customerID);
}
