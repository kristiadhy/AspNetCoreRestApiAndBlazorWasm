using Blazored.LocalStorage;
using Domain.DTO;
using Extension.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Services.Extensions;
using Services.IRepositories;
using System.Text.Json;

namespace Services.Repositories;

public class AuthenticationService : IAuthenticationService
{
    private readonly DefaultApiService _client;
    private readonly JsonSerializerOptions _options;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(DefaultApiService client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _client = client;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<string> GetCurrentTokenFromLocalStorage()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (token is not null)
            return token;
        else
            return string.Empty;
    }

    public async Task<RegistrationResponseDto> RegisterUser(UserRegistrationDTO userForRegistration)
    {
        var registrationResult = await _client.PostAsync("authentication/registration", userForRegistration);
        var registrationContent = await registrationResult.Content.ReadAsStringAsync();

        if (!registrationResult.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<RegistrationResponseDto>(registrationContent, _options);
            return result;
        }

        return new RegistrationResponseDto { IsSuccessfulRegistration = true };
    }

    public async Task<TokenDTO> Login(UserAuthenticationDTO userForAuthentication)
    {
        var authResult = await _client.PostAsync("authentication/login", userForAuthentication);
        var authContent = await authResult.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TokenDTO>(authContent, _options);

        if (!authResult.IsSuccessStatusCode)
            return result;

        await _localStorage.SetItemAsync("authToken", result.AccessToken);
        await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
        ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);

        return result;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");
        ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        _client.RemoveAuthorizationHeader();
    }

    public async Task<string> RefreshToken()
    {
        var AccessToken = await _localStorage.GetItemAsync<string>("authToken");
        var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

        if (string.IsNullOrEmpty(AccessToken) || string.IsNullOrEmpty(refreshToken))
            return string.Empty;

        //var tokenDto = JsonSerializer.Serialize(new TokenDTO(AccessToken, refreshToken));
        var tokenDto = new TokenDTO(AccessToken, refreshToken);

        var refreshResult = await _client.PostAsync("authentication/refresh", tokenDto);
        var refreshContent = await refreshResult.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<TokenDTO>(refreshContent, _options);

        if (!refreshResult.IsSuccessStatusCode)
            throw new ApplicationException("Something went wrong during the refresh token action");

        await _localStorage.SetItemAsync("authToken", result.AccessToken);
        await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

        return result.AccessToken;
    }
}

