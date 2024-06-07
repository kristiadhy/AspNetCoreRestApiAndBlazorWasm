using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/suppliers")]
public class SupplierController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpGet(Name = "Suppliers")]
    public async Task<IActionResult> GetByParameters(Guid supplierID, [FromQuery] SupplierParam supplierParam, CancellationToken cancellationToken)
    {
        var pagedResult = await _serviceManager.SupplierService.GetByParametersAsync(supplierID, supplierParam, false, cancellationToken);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.metaData);
        return Ok(pagedResult.supplierDto);
    }


    [HttpGet("{id:guid}", Name = "SupplierByID")]
    public async Task<IActionResult> GetByID(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await _serviceManager.SupplierService.GetBySupplierIDAsync(id, false, cancellationToken);
        return Ok(supplier);
    }

    [HttpPost(Name = "CreateSupplier")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Create([FromBody] SupplierDto supplierDto, CancellationToken cancellationToken)
    {
        var createdSupplier = await _serviceManager.SupplierService.CreateAsync(supplierDto, false, cancellationToken);
        return CreatedAtRoute("SupplierByID", new { id = createdSupplier.SupplierID }, createdSupplier);
    }

    [HttpPut(Name = "UpdateSupplier")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Update([FromBody] SupplierDto supplierDto, CancellationToken cancellationToken)
    {
        await _serviceManager.SupplierService.UpdateAsync(supplierDto, true, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}", Name = "PartiallyUpdateSupplier")]
    public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid id, [FromBody] JsonPatchDocument<SupplierDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null.");

        var result = await _serviceManager.SupplierService.GetSupplierForPatchAsync(id, true);
        patchDoc.ApplyTo(result.supplierToPatch);
        await _serviceManager.SupplierService.SaveChangesForPatchAsync(result.supplierToPatch, result.supplier);
        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "DeleteSupplier")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _serviceManager.SupplierService.DeleteAsync(id, false, cancellationToken);
        return NoContent();
    }
}