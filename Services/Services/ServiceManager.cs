using Application.IRepositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;
using Services.Contracts;
using Services.Services;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICustomerService> _lazyCustomerService;
    private readonly Lazy<ISupplierService> _lazySupplierService;
    private readonly Lazy<IAuthenticationService> _lazyAuthenticationService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger, UserManager<UserModel> userManager, IConfiguration configuration)
    {
        _lazyCustomerService = new Lazy<ICustomerService>(() => new CustomerService(repositoryManager, mapper, logger));
        _lazySupplierService = new Lazy<ISupplierService>(() => new SupplierService(repositoryManager, mapper, logger));
        _lazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager, mapper, logger, userManager, configuration));
    }

    public ICustomerService CustomerService => _lazyCustomerService.Value;
    public ISupplierService SupplierService => _lazySupplierService.Value;
    public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;
}
