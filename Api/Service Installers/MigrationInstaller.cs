//using Api.ServiceInstallers;
//using Microsoft.EntityFrameworkCore;
//using Persistence.Context;

//namespace Api.ServiceInstallers;

//public class MigrationInstaller : IServiceInstallers
//{
//    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
//    {
//        using var provider = services.AddScoped<AppDBContext>().BuildServiceProvider();
//        using var scope = provider.CreateScope();

//        using AppDBContext dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

//        dbContext.Database.Migrate();
//    }
//}
