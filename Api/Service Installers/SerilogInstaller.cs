//using Api.ServiceInstallers;
//using Serilog;

//namespace BionPro.Api.Service_Installers;

//public class SerilogInstaller : IServiceInstallers
//{
//    public void InstallServices(IServiceCollection services, IConfiguration configuration, IHostBuilder host)
//    {
//        const string logPath = "Logs/appLog-.log";
//        var logger = new LoggerConfiguration()
//            .MinimumLevel.Information()
//            .WriteTo.Console()
//            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
//            .CreateLogger();

//        services.AddSerilog(logger);
//    }
//}
