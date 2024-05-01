namespace Api.ServiceInstallers;

public interface IServiceInstallers
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host);
}
