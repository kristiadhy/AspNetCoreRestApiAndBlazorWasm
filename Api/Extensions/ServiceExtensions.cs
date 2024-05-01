using Api.ServiceInstallers;
using Serilog;
using System.Reflection;

namespace Presentation.Api;

public static class ServiceExtensions
{
    public static IServiceCollection InstallServices(this IServiceCollection services, IConfiguration configuration, IHostBuilder host, params Assembly[] assemblies)

    {
        IEnumerable<IServiceInstallers> serviceInstallers = assemblies.SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IServiceInstallers>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstallers>();

        foreach (IServiceInstallers serviceInstaller in serviceInstallers)
        {
            serviceInstaller.InstallServices(services, configuration, host);
        }

        return services;

        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }

    //public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    //{
    //    services.AddSwaggerGen(options =>
    //    {
    //        options.SwaggerDoc("v1", new OpenApiInfo
    //        {
    //            Title = "BionPro",
    //            Version = "v1"
    //        });

    //        var jwtSecurityScheme = new OpenApiSecurityScheme()
    //        {
    //            In = ParameterLocation.Header,
    //            Description = @"JWT Authorization header using the Bearer scheme. 
    //            Enter your token in the text input below.",
    //            Name = "Authorization", //IMPORTANT! : Please don't change with anything other than Authorization
    //            Type = SecuritySchemeType.Http,
    //            BearerFormat = "JWT",
    //            Scheme = "Bearer"
    //        };

    //        options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    //        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    //              {
    //                {
    //                  new OpenApiSecurityScheme
    //                  {
    //                    Reference = new OpenApiReference
    //                      {
    //                        Type = ReferenceType.SecurityScheme,
    //                        Id = "Bearer"
    //                      }
    //                    },
    //                    new List<string>()
    //                  }
    //                });
    //    });

    //    return services;
    //}

    //public static IServiceCollection ConfigureCORS(this IServiceCollection services)
    //{
    //    services.AddCors(o =>
    //    {
    //        o.AddPolicy("CorsPolicy",
    //            builder => builder.AllowAnyOrigin()
    //            .AllowAnyMethod()
    //            .AllowAnyHeader());
    //    });

    //    return services;
    //}

    //public static void ConfigureMigrations(this IServiceCollection services)
    //{
    //    using var provider = services.AddScoped<AppDBContext>().BuildServiceProvider();
    //    using var scope = provider.CreateScope();

    //    using AppDBContext dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

    //    dbContext.Database.Migrate();
    //}

    //NOTE : This method is to hard-coding the serilog. In this project we use different approach which we put all the serilog configuration in the appsettings.json
    //We better used the second approach for more flexibility. For example, we want to set different minimum level in development environment and production environment, it's easy to implement that using appsettings.json.
    public static IServiceCollection ConfigureSerilog(this IServiceCollection services)
    {
        const string logPath = "Logs/appLog-.log";

        //var logger = new LoggerConfiguration()
        //    .MinimumLevel.Information()
        //    .Enrich.FromLogContext()
        //    .WriteTo.Console()
        //    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
        //    .CreateLogger();

        //services.AddSerilog(logger);

        //-----

        //Log.Logger = new LoggerConfiguration()
        //    .MinimumLevel.Information()
        //    .Enrich.FromLogContext()
        //    .WriteTo.Console()
        //    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
        //    .CreateLogger();

        //services.AddLogging(b => b.AddSerilog(dispose: true));

        //-----

        Serilog.ILogger logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        services.AddSingleton(logger);

        return services;
    }
}
