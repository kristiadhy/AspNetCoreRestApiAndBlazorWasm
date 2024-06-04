using Api.ServiceInstallers;

namespace Api.Service_Installers;

public class WebAssemblyHostedInstaller : IServiceInstallers
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {
        services.AddRazorPages();
    }
}
