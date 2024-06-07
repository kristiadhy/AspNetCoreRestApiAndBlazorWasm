
using Services.Repositories;

namespace Services.IRepositories;

public interface IServiceManager
{
    ICustomerService CustomerService { get; }
    ISupplierService SupplierService { get; }
    IAuthenticationService AuthService { get; }
    RefreshTokenService RefreshTokenService { get; }
    HttpInterceptorService InterceptorService { get; }
}
