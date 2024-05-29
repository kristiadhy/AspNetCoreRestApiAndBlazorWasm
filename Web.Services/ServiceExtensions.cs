using Blazored.LocalStorage;
using Extension.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Services.Extensions;
using Services.IRepositories;
using Services.Repositories;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Services;

//This class is used to register services from contract layer

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureHTTPServices(this IServiceCollection services)
    {
        services.AddScoped(sp => new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
        });

        services.AddBlazoredLocalStorage();
        services.AddHttpClient<DefaultApiService>();
        services.AddHttpClientInterceptor(); //Should be put below the http client registration
        services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddAuthorizationCore();

        return services;

        //services.AddHttpClient("_", options =>
        //{
        //    //IMPORTANT : Don't add additional API path like https://localhost:7229/api because the system won't recognize the additional path. It will be read as https://localhost:7229
        //    options.BaseAddress = new Uri("https://localhost:7229/api/");
        //    options.DefaultRequestHeaders.Clear();
        //}
        //);
        //services.AddScoped(sp => sp.GetService<IHttpClientFactory>()!.CreateClient("_"));
    }
}
