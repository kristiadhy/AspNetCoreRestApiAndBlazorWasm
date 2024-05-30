using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Services.IRepositories;
using WebAssembly.Services;
using WebAssembly.StateManagement;

namespace WebAssembly.Pages;

public partial class CustomerL
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
    CustomerState CustomerState { get; set; } = default!;

    internal static RadzenDataGrid<CustomerDTO> CustomerGrid { get; set; } = default!;

    protected bool isLoading = false;
    //IList<CustomerDTO>? selectedRow;

    //protected override async Task OnInitializedAsync()
    //{
    //    await base.OnInitializedAsync();

    //    selectedRow = CustomerState.CustomerList.Take(1).ToList();
    //}

    protected async Task EvReloadData()
    {
        await EvLoadData();
        await CustomerGrid.Reload();
    }

    protected async Task EvLoadData()
    {
        isLoading = true;

        await Task.Yield();

        await CustomerState.LoadCustomers();

        isLoading = false;
    }

    protected void EvEditRow(CustomerDTO customer)
    {
        NavigationManager.NavigateTo($"customer/edit/{customer.CustomerID}");
    }

    protected async Task EvDeleteRow(CustomerDTO customer)
    {
        if (customer is null)
            return;

        string customerName = customer.CustomerName ?? string.Empty;
        bool confirmationStatus = await ConfirmationModalService.DeleteConfirmation("Customer", customerName);
        if (!confirmationStatus)
            return;

        Guid customerID = (Guid)customer.CustomerID!;
        var response = await ServiceManager.CustomerService.Delete(customerID);
        if (!response.IsSuccessStatusCode)
            return;

        NotificationService.DeleteNotification("Customer has been deleted");
        await CustomerState.LoadCustomers();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"customer/create");
    }
}
