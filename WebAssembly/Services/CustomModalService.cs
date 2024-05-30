using Radzen;

namespace WebAssembly.Services;

public class CustomModalService
{
    public DialogService DialogService { get; }

    public CustomModalService(DialogService dialogService)
    {
        DialogService = dialogService;
    }

    public async Task<bool> SavingConfirmation(string transactionName)
    {
        bool? confirmationStatus = await DialogService.Confirm($"Do you want to save this {transactionName}?", $"Save {transactionName}", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirmationStatus is not null)
            if ((bool)confirmationStatus)
                return true;

        return false;
    }

    public async Task<bool> DeleteConfirmation(string transactionName, string entityName)
    {
        bool? confirmationStatus = await DialogService.Confirm($"{entityName} will be deleted. Are you sure want to do this?", $"Delete {transactionName}", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (confirmationStatus is not null)
            if ((bool)confirmationStatus)
                return true;

        return false;
    }
}
