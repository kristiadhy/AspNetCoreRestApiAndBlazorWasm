using Blazored.LocalStorage;
using Extension.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Extensions;
using Services.IRepositories;
using Services.Repositories;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Services;

//This class is used to register services from contract layer

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureHTTPServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(sp => new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
        });

        services.AddBlazoredLocalStorage();

        services.Configure<ApiConfiguration>(configuration.GetSection("ApiConfiguration"));
        services.AddHttpClient<CustomHttpClient>((sp, cl) =>
        {
            var apiConfiguration = sp.GetRequiredService<IOptions<ApiConfiguration>>();
            cl.BaseAddress = new Uri($"{apiConfiguration.Value.BaseAddress}/api/");
        });
        services.AddHttpClientInterceptor(); //Should be put below the http client registration
        services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddAuthorizationCore();

        return services;

        //services.AddHttpClient("_", options =>
        //{
        //    options.BaseAddress = new Uri("https://localhost:7229/api/");
        //    options.DefaultRequestHeaders.Clear();
        //}
        //);
        //services.AddScoped(sp => sp.GetService<IHttpClientFactory>()!.CreateClient("_"));
    }
}
