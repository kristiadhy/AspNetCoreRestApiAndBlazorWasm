using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Services.Extensions;
using Services.IRepositories;
using Toolbelt.Blazor;

namespace Services.Repositories;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICustomerService> _lazyCustomerService;
    private readonly Lazy<IAuthenticationService> _lazyAuthService;
    private readonly Lazy<RefreshTokenService> _lazyRefreshTokenService;
    private readonly Lazy<HttpInterceptorService> _lazyInterceptorService;

    public ServiceManager(DefaultApiService apiService, JsonSerializerSettings settings, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, HttpClientInterceptor httpClientInterceptor)
    {
        _lazyCustomerService = new Lazy<ICustomerService>(() => new CustomerService(apiService, settings));
        _lazyAuthService = new Lazy<IAuthenticationService>(() => new AuthenticationService(apiService, authStateProvider, localStorage));
        _lazyRefreshTokenService = new Lazy<RefreshTokenService>(() => new RefreshTokenService(authStateProvider, _lazyAuthService.Value));
        _lazyInterceptorService = new Lazy<HttpInterceptorService>(() => new HttpInterceptorService(httpClientInterceptor, _lazyRefreshTokenService.Value));
    }

    public ICustomerService CustomerService => _lazyCustomerService.Value;
    public IAuthenticationService AuthService => _lazyAuthService.Value;
    public RefreshTokenService RefreshTokenService => _lazyRefreshTokenService.Value;
    public HttpInterceptorService InterceptorService => _lazyInterceptorService.Value;
}
