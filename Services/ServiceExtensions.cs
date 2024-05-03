using Microsoft.Extensions.DependencyInjection;
using Services.Contracts;

namespace Services;

//This class is used to register services from contract layer

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureServiceServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();

        return services;
    }
}
