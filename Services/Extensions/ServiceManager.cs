using Application.Repositories;
using AutoMapper;
using Serilog;
using Services.Contracts;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICustomerService> _lazyCustomerService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _lazyCustomerService = new Lazy<ICustomerService>(() => new CustomerService(repositoryManager, mapper, logger));
    }

    public ICustomerService CustomerService => _lazyCustomerService.Value;
}
