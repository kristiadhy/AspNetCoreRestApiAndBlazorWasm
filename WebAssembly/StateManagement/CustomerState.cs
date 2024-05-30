using Domain.DTO;
using Domain.Parameters;
using Services.IRepositories;

namespace WebAssembly.StateManagement;

public class CustomerState
{
    private IServiceManager _serviceManager;
    public List<CustomerDTO> CustomerList { get; set; } = new();
    public MetaData MetaData { get; set; } = new();
    public CustomerParam CustomerParameter { get; set; } = new();
    public CustomerDTO Customer { get; set; } = new();

    public CustomerState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadCustomers()
    {
        var pagingResponse = await _serviceManager.CustomerService.GetCustomers(CustomerParameter);
        CustomerList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }
}
