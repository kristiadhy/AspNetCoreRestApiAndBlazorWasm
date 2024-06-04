using Application.Exceptions;
using Application.Repositories;
using Application.Validators;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Serilog;
using Services.Contracts;

namespace Services;

internal sealed class CustomerService : ICustomerService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CustomerService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<CustomerDTO> customerDTO, MetaData metaData)> GetByParametersAsync(Guid customerID, CustomerParam customerParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get customers");

        var customers = await _repositoryManager.CustomerRepo.GetByParametersAsync(customerParam, trackChanges);
        //if (!customers.Any())
        //    throw new NoCustomerFoundException();

        var customersToReturn = _mapper.Map<IEnumerable<CustomerDTO>>(customers);

        return (customersToReturn, customers.MetaData);
    }

    public async Task<CustomerDTO> GetByCustomerIDAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get customer with ID : {customerID}");

        var customer = await _repositoryManager.CustomerRepo.GetByIDAsync(customerID, trackChanges);
        if (customer is null)
            throw new CustomerIDNotFoundException(customerID);

        var customersToReturn = _mapper.Map<CustomerDTO>(customer);
        return customersToReturn;
    }

    public async Task<CustomerDTO> CreateAsync(CustomerDTO customerDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var customerMDForValidation = _mapper.Map<CustomerMD>(customerDto);
        var validator = new CustomerValidator();
        validator.ValidateInput(customerMDForValidation);

        _logger.Information("Insert new customer : {customerName}", customerDto.CustomerName);

        var customerMD = _mapper.Map<CustomerMD>(customerDto);
        _repositoryManager.CustomerRepo.CreateEntity(customerMD, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        var customerToReturn = _mapper.Map<CustomerDTO>(customerMD);

        return customerToReturn;
    }

    public async Task UpdateAsync(CustomerDTO customerDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var customerMDForValidation = _mapper.Map<CustomerMD>(customerDto);
        var validator = new CustomerValidator();
        validator.ValidateInput(customerMDForValidation);

        _logger.Information("Update customer : {customerName}", customerDto.CustomerName);

        CustomerMD customerToUpdate = _mapper.Map<CustomerMD>(customerDto);
        _repositoryManager.CustomerRepo.UpdateEntity(customerToUpdate, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        //var lstCustomerMD = ServiceExtensions<CustomerMD, CustomerDTO, CustomerQP>.AddEntityListfromQP(QP);
        //var lstCustomerMD = DataProcessingExtension.Create_ListOfModel_FromListOfDTO<CustomerMD, CustomerDTO>(lstCustomerDTOs, _mapper);

        //foreach (var row in lstCustomerDTOs)
        //    _logger.LogInformation("Delete customer : {customerName}", row.CustomerName);

        var customerForDelete = await _repositoryManager.CustomerRepo.GetByIDAsync(customerID, trackChanges);
        if (customerForDelete is null)
            throw new CustomerIDNotFoundException(customerID);

        _logger.Information("Delete customer : {customerName}", customerForDelete.CustomerName);

        _repositoryManager.CustomerRepo.DeleteEntity(customerForDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task<(CustomerDTO customerToPatch, CustomerMD customer)> GetCustomerForPatchAsync(Guid customerID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        var customer = await _repositoryManager.CustomerRepo.GetByIDAsync(customerID, empTrackChanges);
        if (customer is null)
            throw new CustomerIDNotFoundException(customerID);

        var customerToPatch = _mapper.Map<CustomerDTO>(customer);

        return (customerToPatch, customer);
    }

    public async Task SaveChangesForPatchAsync(CustomerDTO customerToPatch, CustomerMD customer, CancellationToken cancellationToken = default)
    {
        _mapper.Map(customerToPatch, customer);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }
}
