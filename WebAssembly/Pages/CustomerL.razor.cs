using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Domain.DTO;
using WebAssembly.Services;
using Services.IRepositories;
using Domain.Parameters;

namespace WebAssembly.Pages;

public partial class CustomerL
{
    [Inject]
    NavigationManager NavigationHelper { get; set; } = default!;
    [Inject]
    CustomNotificationService NotificationService { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;

    protected RadzenDataGrid<CustomerDTO> customerGrid = default!;
    protected int count;
    public List<CustomerDTO> CustomerList { get; set; } = default!;
    public MetaData MetaData { get; set; } = default!;

    protected CustomerParam customerParameter = default!;
    protected bool isLoading = false;
    protected IEnumerable<string> selectedTitles = default!;

    protected async Task EvLoadData()
    {
        isLoading = true;

        await Task.Yield();

        var pagingResponse = await ServiceManager.CustomerService.Read(customerParameter);
        CustomerList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;

        isLoading = false;
    }

    protected async Task OnSelectedTitlesChange(object value)
    {
        if (selectedTitles != null && !selectedTitles.Any())
            selectedTitles = null;

        await customerGrid.FirstPage();
    }

    protected async Task Reset()
    {
        customerGrid.Reset(true);
        await customerGrid.FirstPage(true);
    }

    protected void EvCreateNew()
    {
        NavigationHelper.NavigateTo($"customer/create");
    }
}
