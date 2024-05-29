using Domain.DTO;

namespace WebAssembly.StateManagement;

public class CustomerState
{
    public List<CustomerDTO> CustomerList { get; set; } = new();
    public CustomerDTO Customer { get; set; } = new();
}
