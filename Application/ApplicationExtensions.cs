using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

//This class is used to register services from application layer

public static class ApplicationExtensions
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
