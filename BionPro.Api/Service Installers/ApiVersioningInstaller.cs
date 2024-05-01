using Microsoft.AspNetCore.Mvc;

namespace Api.ServiceInstallers;

public class ApiVersioningInstaller : IServiceInstallers
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
        });
    }
}
