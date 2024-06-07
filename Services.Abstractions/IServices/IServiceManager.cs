
namespace Services.Contracts;

public interface IServiceManager
{
    ICustomerService CustomerService { get; }
    ISupplierService SupplierService { get; }
    IAuthenticationService AuthenticationService { get; }

}
