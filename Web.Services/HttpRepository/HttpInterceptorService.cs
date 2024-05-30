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

        //IMPORTANT : How refresh token work.
        //1. We must include the JWT token bearer every time we make a call to the web API service; The token should be active, it means we can't use an expired token.
        //2. TryRefreshToken() is checking whether the token is expired or not (We know it because we save the expiry time in the JWT token)
        //3. When the token is nearly expired, then extend the expiration time (In this method, we extend token expiry time when the expiration time remaining is < 5 minutes).

        if (!absPath.Contains("authentication"))
        {
            var accessToken = await _refreshTokenService.TryRefreshToken();
            e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
        }
    }

    public void DisposeEvent() => _interceptor.BeforeSendAsync -= InsertNewRefreshTokenOnRequestHeader;
}
