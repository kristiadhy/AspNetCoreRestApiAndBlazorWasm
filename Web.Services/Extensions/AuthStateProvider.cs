using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Extension.Services;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationState _anonymous;

    public AuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        //Without any parameter in the ClaimsIdentity, it won't recognized as Authorized user.
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    //GetAuthenticationStateAsync() will be called when we use Authorize view in the UI
    //This method will be called in the initialization of this class and refreshtokenservice -> TryRefreshToken()
    //It doesn't have a reference to this method caller because it's use the parent's method which is in the AuthenticationStateProvider

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //Get token from local storage
        var token = await _localStorage.GetItemAsync<string>("authToken");
        //string? token = null;
        if (string.IsNullOrWhiteSpace(token))
            return _anonymous;

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
    }

    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState);
    }
}

