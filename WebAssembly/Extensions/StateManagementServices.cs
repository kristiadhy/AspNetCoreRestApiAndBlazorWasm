using WebAssembly.StateManagement;

namespace WebAssembly.Extensions;

public static class StateManagementServices
{
    public static IServiceCollection ConfigureStateManagementServices(this IServiceCollection services)
    {
        services.AddScoped<CustomerState>();

        return services;
    }
}
