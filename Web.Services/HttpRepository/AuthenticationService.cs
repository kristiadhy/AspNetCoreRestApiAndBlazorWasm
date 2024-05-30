using Blazored.LocalStorage;
using Domain.DTO;
using Extension.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Services.Extensions;
using Services.IRepositories;

namespace Services.Repositories;

public class AuthenticationService : IAuthenticationService
{
    private readonly DefaultApiService _client;
    private readonly JsonSerializerSettings _options;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(DefaultApiService client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<string> GetCurrentTokenFromLocalStorage()
    {
        var accessToken = await _localStorage.GetItemAsync<string>("authToken");
        if (accessToken is not null)
            return accessToken;
        else
            return string.Empty;
    }

    public async Task<ResponseDto> RegisterUser(UserRegistrationDTO userForRegistration)
    {
        var response = await _client.PostAsync("authentication/registration", userForRegistration);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponse(response, content, _options);

        return new ResponseDto { IsSuccess = true };
    }

    public async Task<TokenDTO> Login(UserAuthenticationDTO userForAuthentication)
    {
        var response = await _client.PostAsync("authentication/login", userForAuthentication);
        var content = await response.Content.ReadAsStringAsync();

        _client.CheckErrorResponse(response, content, _options);

        var result = JsonConvert.DeserializeObject<TokenDTO>(content, _options);

        await _localStorage.SetItemAsync("authToken", result!.AccessToken);
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
        var accessToken = await _localStorage.GetItemAsync<string>("authToken");
        var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            return string.Empty;

        var tokenDto = new TokenDTO(accessToken, refreshToken);

        var response = await _client.PostAsync("authentication/refresh", tokenDto);
        var content = await response.Content.ReadAsStringAsync();

        _client.CheckErrorResponse(response, content, _options);
        if (content is null)
            throw new ApplicationException("Something wrong with the refresh token API response");

        var result = JsonConvert.DeserializeObject<TokenDTO>(content, _options);

        await _localStorage.SetItemAsync("authToken", result!.AccessToken);
        await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

        return result.AccessToken;
    }
}

