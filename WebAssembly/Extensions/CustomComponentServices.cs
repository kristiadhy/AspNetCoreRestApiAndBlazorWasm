using WebAssembly.Services;

namespace WebAssembly.Extensions;

public static class CustomComponentServices
{
    public static IServiceCollection ConfigureCustomComponentServices(this IServiceCollection services)
    {
        services.AddScoped<CustomModalService>();
        services.AddScoped<CustomNotificationService>();

        return services;
    }
}
