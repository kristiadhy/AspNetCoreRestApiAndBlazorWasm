using Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories;

namespace Persistence;

//This class is used to register services from infrastructure layer

public static class PersistenceExtensions
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDBContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DBConnection"));
        });

        services.AddScoped<IUnitOfWorkRepo, UnitOfWorkRepo>();
        services.AddScoped<ICustomerRepo, CustomerRepo>();

        services.AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }
}
