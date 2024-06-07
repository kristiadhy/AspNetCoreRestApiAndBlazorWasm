using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Services.IRepositories;
using WebAssembly.Services;
using WebAssembly.StateManagement;

namespace WebAssembly.Pages;

public partial class SupplierDisplay
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    CustomNotificationService NotificationService { get; set; } = default!;
    [Inject]
    CustomModalService ConfirmationModalService { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;
    [Inject]
    SupplierState SupplierState { get; set; } = default!;

    internal static RadzenDataGrid<SupplierDto> SupplierGrid { get; set; } = default!;

    protected bool isLoading = false;

    protected async Task EvReloadData()
    {
        await EvLoadData();
        await SupplierGrid.Reload();
    }

    protected async Task EvLoadData()
    {
        isLoading = true;

        await Task.Yield();

        await SupplierState.LoadSuppliers();

        isLoading = false;
    }

    protected void EvEditRow(SupplierDto supplier)
    {
        NavigationManager.NavigateTo($"supplier/edit/{supplier.SupplierID}");
    }

    protected async Task EvDeleteRow(SupplierDto supplier)
    {
        if (supplier is null)
            return;

        string supplierName = supplier.SupplierName ?? string.Empty;
        bool confirmationStatus = await ConfirmationModalService.DeleteConfirmation("Supplier", supplierName);
        if (!confirmationStatus)
            return;

        Guid supplierID = (Guid)supplier.SupplierID!;
        var response = await ServiceManager.SupplierService.Delete(supplierID);
        if (!response.IsSuccessStatusCode)
            return;

        NotificationService.DeleteNotification("Supplier has been deleted");
        await SupplierState.LoadSuppliers();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"supplier/create");
    }
}
