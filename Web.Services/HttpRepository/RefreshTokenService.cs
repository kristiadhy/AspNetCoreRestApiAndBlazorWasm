using Microsoft.AspNetCore.Components.Authorization;
using Services.IRepositories;


namespace Services.Repositories;

public class RefreshTokenService
{
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly IAuthenticationService _authService;

    public RefreshTokenService(AuthenticationStateProvider authStateProvider, IAuthenticationService authService)
    {
        _authStateProvider = authStateProvider;
        _authService = authService;
    }

    public async Task<string> RefreshTokenOrUseExistingToken()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (user.Identity is not null)
            if (user.Identity.IsAuthenticated)
            {
                var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
                var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
                var timeUTC = DateTime.UtcNow;
                var diff = expTime - timeUTC;
                if (diff.TotalMinutes <= 5)
                    return await _authService.RefreshToken();
                else
                    return await _authService.GetCurrentTokenFromLocalStorage();
            }

        return string.Empty;
    }
}
