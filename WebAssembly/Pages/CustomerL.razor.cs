using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen;
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
    IServiceManager ServiceManager { get; set; } = default!;
    [Inject]
    CustomerState CustomerState { get; set; } = default!;

    internal static RadzenDataGrid<CustomerDTO> CustomerGrid { get; set; } = default!;
    protected int TotalCount;
    public MetaData MetaData { get; set; } = default!;

    protected CustomerParam customerParameter = new();
    protected bool isLoading = false;
    protected IEnumerable<string> selectedTitles = default!;
    protected int PageSize = 1;

    protected override void OnInitialized()
    {
        ServiceManager.InterceptorService.RegisterEvent();
    }

    protected async Task EvReloadData()
    {
        await CustomerGrid.Reload();
    }

    protected async Task EvLoadData(LoadDataArgs args)
    {
        isLoading = true;

        await Task.Yield();

        var pagingResponse = await ServiceManager.CustomerService.GetCustomers(customerParameter);

        CustomerState.CustomerList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
        PageSize = MetaData.PageSize;
        TotalCount = MetaData.TotalCount;

        isLoading = false;
    }

    protected void EvEditRow(CustomerDTO customer)
    {
        NavigationManager.NavigateTo($"customer/edit/{customer.CustomerID}");
    }

    protected void EvDeleteRow(CustomerDTO customer)
    {

    }

    protected async Task OnSelectedTitlesChange(object value)
    {
        if (selectedTitles != null && !selectedTitles.Any())
            selectedTitles = null;

        await CustomerGrid.FirstPage();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"customer/create");
    }

    public void Dispose()
    {
        ServiceManager.InterceptorService.DisposeEvent();
    }
}
