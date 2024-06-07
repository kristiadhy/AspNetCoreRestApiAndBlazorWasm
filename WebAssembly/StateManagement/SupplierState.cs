using Domain.DTO;
using Domain.Parameters;
using Services.IRepositories;

namespace WebAssembly.StateManagement;

public class SupplierState
{
    private IServiceManager _serviceManager;
    public List<SupplierDto> SupplierList { get; set; } = new();
    public MetaData MetaData { get; set; } = new();
    public SupplierParam SupplierParameter { get; set; } = new();
    public SupplierDto Supplier { get; set; } = new();

    public SupplierState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadSuppliers()
    {
        var pagingResponse = await _serviceManager.SupplierService.GetSuppliers(SupplierParameter);
        SupplierList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }
}
