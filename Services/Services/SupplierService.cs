using Application.Exceptions;
using Application.IRepositories;
using Application.Validators;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Serilog;
using Services.Contracts;

namespace Services;

internal sealed class SupplierService : ISupplierService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SupplierService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<SupplierDto> supplierDto, MetaData metaData)> GetByParametersAsync(Guid supplierID, SupplierParam supplierParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get suppliers");

        var suppliers = await _repositoryManager.SupplierRepo.GetByParametersAsync(supplierParam, trackChanges);

        var suppliersToReturn = _mapper.Map<IEnumerable<SupplierDto>>(suppliers);

        return (suppliersToReturn, suppliers.MetaData);
    }

    public async Task<SupplierDto> GetBySupplierIDAsync(Guid supplierID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get supplier with ID : {supplierID}");

        var suppliers = await _repositoryManager.SupplierRepo.GetByIDAsync(supplierID, trackChanges);
        if (suppliers is null)
            throw new SupplierIDNotFoundException(supplierID);

        var suppliersToReturn = _mapper.Map<SupplierDto>(suppliers);
        return suppliersToReturn;
    }

    public async Task<SupplierDto> CreateAsync(SupplierDto suppliersDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var supplierForValidation = _mapper.Map<SupplierModel>(suppliersDto);
        var validator = new SupplierValidator();
        validator.ValidateInput(supplierForValidation);

        _logger.Information("Insert new suppliers : {suppliersName}", suppliersDto.SupplierName);

        var supplierModel = _mapper.Map<SupplierModel>(suppliersDto);
        _repositoryManager.SupplierRepo.CreateEntity(supplierModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        var suppliersToReturn = _mapper.Map<SupplierDto>(supplierModel);

        return suppliersToReturn;
    }

    public async Task UpdateAsync(SupplierDto supplierDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var supplierForValidation = _mapper.Map<SupplierModel>(supplierDto);
        var validator = new SupplierValidator();
        validator.ValidateInput(supplierForValidation);

        _logger.Information("Update suppliers : {suppliersName}", supplierDto.SupplierName);

        SupplierModel suppliersToUpdate = _mapper.Map<SupplierModel>(supplierDto);
        _repositoryManager.SupplierRepo.UpdateEntity(suppliersToUpdate, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid suppliersID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var supplierForDelete = await _repositoryManager.SupplierRepo.GetByIDAsync(suppliersID, trackChanges);
        if (supplierForDelete is null)
            throw new SupplierIDNotFoundException(suppliersID);

        _logger.Information("Delete suppliers : {supplierName}", supplierForDelete.SupplierName);

        _repositoryManager.SupplierRepo.DeleteEntity(supplierForDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task<(SupplierDto supplierToPatch, SupplierModel supplier)> GetSupplierForPatchAsync(Guid suppliersID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        var supplier = await _repositoryManager.SupplierRepo.GetByIDAsync(suppliersID, empTrackChanges);
        if (supplier is null)
            throw new SupplierIDNotFoundException(suppliersID);

        var supplierToPatch = _mapper.Map<SupplierDto>(supplier);

        return (supplierToPatch, supplier);
    }

    public async Task SaveChangesForPatchAsync(SupplierDto supplierToPatch, SupplierModel supplier, CancellationToken cancellationToken = default)
    {
        _mapper.Map(supplierToPatch, supplier);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }
}
