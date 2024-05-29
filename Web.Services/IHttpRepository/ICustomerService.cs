using Domain.DTO;
using Domain.Parameters;
using Features;

namespace Services.IRepositories;

public interface ICustomerService
{
    public Task<PagingResponse<CustomerDTO>> Read(CustomerParam customerParameter);
    public Task<HttpResponseMessage> Create(CustomerDTO Lst);
    public Task<HttpResponseMessage> Update(CustomerDTO Cls);
    public Task<HttpResponseMessage> Delete(List<CustomerDTO> Lst);
}
