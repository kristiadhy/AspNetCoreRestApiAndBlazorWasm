using Radzen;

namespace WebAssembly.Services;

public class CustomModalService
{
    public DialogService DialogService { get; }

    public CustomModalService(DialogService dialogService)
    {
        DialogService = dialogService;
    }

    public async Task<bool> SavingConfirmation()
    {
        bool? confirmationStatus = await DialogService.Confirm("Do you want to save this transaction?", "Saving Transaction", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirmationStatus is not null)
            if ((bool)confirmationStatus)
                return true;

        return false;
    }

    public async Task<bool> DeleteConfirmation()
    {
        bool? confirmationStatus = await DialogService.Confirm("Do you want to delete this transaction?", "Deleting Transaction", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirmationStatus is not null)
            if ((bool)confirmationStatus)
                return true;

        return false;
    }
}
