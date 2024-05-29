using WebAssembly.Services;
using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Services.IRepositories;

namespace WebAssembly.Pages;

public partial class CustomerT : IDisposable
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    CustomModalService ConfirmationModalService { get; set; } = default!;
    [Inject]
    CustomNotificationService NotificationService { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;

    [Parameter] public Guid? customerID { get; set; }

    protected bool IsSaving = false;
    protected CustomerDTO Customer = new();
    protected CustomerParam CustomerParameter = new();

    protected override void OnInitialized()
    {
        ServiceManager.InterceptorService.RegisterEvent();
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"customer");
    }

    public async Task SubmitAsync(CustomerDTO customer)
    {
        bool confirmationStatus = await ConfirmationModalService.SavingConfirmation();
        if (confirmationStatus)
        {
            IsSaving = true;
            StateHasChanged();

            var response = await ServiceManager.CustomerService.Create(customer);
            if (response.IsSuccessStatusCode)
                NotificationService.SaveNotification("A new customer added");

            IsSaving = false;
        }
    }

    public void ClearField()
    {
        Console.WriteLine("Clear fields");
    }

    public void Dispose()
    {
        ServiceManager.InterceptorService.DisposeEvent();
    }
}
