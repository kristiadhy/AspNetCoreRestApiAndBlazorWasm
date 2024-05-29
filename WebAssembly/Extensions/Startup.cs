using Radzen;
using Services;
using WebAssembly.Extensions;

namespace Extensions;

public static class Startup
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        //Register HTTP service project
        services.ConfigureHTTPServices();

        services.AddRadzenComponents();
        services.AddCascadingAuthenticationState();

        //Register the local services
        services.ConfigureCustomComponentServices();
        services.ConfigureStateManagementServices();

        return services;
    }
}
