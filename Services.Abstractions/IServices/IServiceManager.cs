
namespace Services.Contracts;

public interface IServiceManager
{
    ICustomerService CustomerService { get; }
    IAuthenticationService AuthenticationService { get; }

}
