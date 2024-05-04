using Api.Middleware;
using Api.ServiceInstallers;
using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Persistence;
using Presentation.ActionFilters;
using Presentation.Api;
using Serilog;
using Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {
        //We use another approach to register the services. I Get it from Milan Jovanovic on his youtube channel : https://youtu.be/tKEF6xaeoig?si=o82cy17HOxpjDtAU 
        services.InstallServices(configuration, host, typeof(IServiceInstallers).Assembly);

        //Use the code below if you want to use configuration from appsettings.json
        //Reference : Milan Jovanovic's youtube video about serilog : https://www.youtube.com/watch?v=nVAkSBpsuTk
        host.UseSerilog((context, options) => options.ReadFrom.Configuration(context.Configuration));

        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();

        //Configure service on another layers
        services.ConfigureApplicationServices();
        services.ConfigureInfrastructureServices(configuration);
        services.ConfigureServiceServices();

        //Local function for patch
        NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
            new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider()
            .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();

        services.AddScoped<ValidationFilterAttribute>();

        //Register the controllers.It is the third layer of Clean Architecture
        services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
            config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
        }).AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

        //See the explanation of this code below in this E-Book : ultimate in asp.Net Core web API by Code Maze, page 115 
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        //Register global exception handling
        services.AddTransient<ExceptionHandlingMiddleware>();

        services.AddAuthentication();

        return services;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static IApplicationBuilder UseAppConfigurations(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        //Use global exception handling middleware
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture.Api v1"));

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors("CorsPolicy");

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
