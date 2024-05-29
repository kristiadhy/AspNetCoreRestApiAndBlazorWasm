using Radzen;

namespace WebAssembly.Services;

public class CustomNotificationService
{
    public NotificationService NotificationService { get; }

    public CustomNotificationService(NotificationService notificationService)
    {
        NotificationService = notificationService;
    }

    public void SaveNotification(string detailText)
    {
        NotificationService.Notify(
           new NotificationMessage
           {
               Severity = NotificationSeverity.Info,
               Duration = 3000,
               Summary = "Saved Successfully",
               Detail = detailText
           });
    }
}
