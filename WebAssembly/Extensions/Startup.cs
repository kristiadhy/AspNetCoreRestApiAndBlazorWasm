using WebAssembly.Services;
using Radzen;
using Services;

namespace Extensions;

public static class Startup
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        //Register HTTP service project
        services.ConfigureHTTPServices();
        //Register Radzen to the services
        services.AddRadzenComponents();
        //Register the local service
        services.AddScoped<CustomModalService>();
        services.AddScoped<CustomNotificationService>();
        services.AddCascadingAuthenticationState();
        services.AddAuthorizationCore();

        return services;
    }
}
