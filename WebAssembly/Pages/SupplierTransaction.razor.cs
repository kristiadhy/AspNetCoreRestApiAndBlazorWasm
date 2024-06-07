using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Services.IRepositories;
using WebAssembly.Services;
using WebAssembly.StateManagement;

namespace WebAssembly.Pages;

public partial class SupplierTransaction
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    CustomModalService ConfirmationModalService { get; set; } = default!;
    [Inject]
    CustomNotificationService NotificationService { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;
    [Inject]
    SupplierState SupplierState { get; set; } = default!;

    [Parameter] public Guid? ParamSupplierID { get; set; }

    protected string PagePathText = string.Empty;
    protected string FormHeaderText = string.Empty;
    protected GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    protected bool IsSaving = false;
    protected SupplierParam SupplierParameter = new();

    protected override async Task OnParametersSetAsync()
    {
        if (ParamSupplierID is not null)
        {
            SupplierState.Supplier = await ServiceManager.SupplierService.GetSupplierByID((Guid)ParamSupplierID);

            PagePathText = GlobalEnum.FormStatus.Edit.ToString();
            FormHeaderText = $"{GlobalEnum.FormStatus.Edit.ToString()} Existing Supplier";
            FormStatus = GlobalEnum.FormStatus.Edit;
        }
        else
        {
            PagePathText = GlobalEnum.FormStatus.New.ToString();
            FormHeaderText = $"Create {GlobalEnum.FormStatus.New.ToString()} Supplier";
            FormStatus = GlobalEnum.FormStatus.New;
        }
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"supplier");
    }

    public async Task SubmitAsync(SupplierDto supplier)
    {
        bool confirmationStatus = await ConfirmationModalService.SavingConfirmation("Supplier");
        if (!confirmationStatus)
            return;

        IsSaving = true;
        StateHasChanged();

        if (FormStatus == GlobalEnum.FormStatus.New)
        {
            supplier.SupplierID = null;
            var response = await ServiceManager.SupplierService.Create(supplier);
            if (response.IsSuccessStatusCode)
                NotificationService.SaveNotification("A new supplier added");
        }
        else if (FormStatus == GlobalEnum.FormStatus.Edit)
        {
            var response = await ServiceManager.SupplierService.Update(supplier);
            if (response.IsSuccessStatusCode)
            {
                NotificationService.SaveNotification("Supplier updated");
            }
        }

        //Load customer state after making changes
        await SupplierState.LoadSuppliers();

        IsSaving = false;
    }

    public void ClearField()
    {
        Console.WriteLine("Clear fields");
    }
}
