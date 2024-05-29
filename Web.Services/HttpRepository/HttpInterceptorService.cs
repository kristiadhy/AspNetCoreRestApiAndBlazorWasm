using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace Services.Repositories;

public class HttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly RefreshTokenService _refreshTokenService;

    public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService)
    {
        _interceptor = interceptor;
        _refreshTokenService = refreshTokenService;
    }

    public void RegisterEvent() => _interceptor.BeforeSendAsync += InsertNewRefreshTokenOnRequestHeader;

    public async Task InsertNewRefreshTokenOnRequestHeader(object sender, HttpClientInterceptorEventArgs e)
    {
        var absPath = e.Request.RequestUri!.AbsolutePath;

        //We need to check the token lifetime every time we call a service to the web API. Uri path that contains "authentication" means that we call the authentication API service (Register, Login, and RefreshToken). Hence, we don't need to insert a token in the HTTP client header.
        if (!absPath.Contains("authentication"))
        {
            var tokenUse = await _refreshTokenService.RefreshTokenOrUseExistingToken();
            e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", tokenUse);
        }
    }

    public void DisposeEvent() => _interceptor.BeforeSendAsync -= InsertNewRefreshTokenOnRequestHeader;
}
