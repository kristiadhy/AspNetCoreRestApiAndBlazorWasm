﻿using Application.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Services.Contracts;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICustomerService> _lazyCustomerService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _lazyCustomerService = new Lazy<ICustomerService>(() => new CustomerService(repositoryManager, mapper));
    }

    public ICustomerService CustomerService => _lazyCustomerService.Value;
}