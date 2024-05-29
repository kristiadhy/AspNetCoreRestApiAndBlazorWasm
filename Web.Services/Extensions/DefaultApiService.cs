using Blazored.LocalStorage;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Services.Extensions;
public class DefaultApiService
{
    //IMPORTANT : Here is the logic of how we use the httpclient service
    //1. We register httpclient as class/service and set all of the configuration here in the constructor.

    private readonly HttpClient HttpClient;
    private readonly ILocalStorageService _localStorage;

    public DefaultApiService(HttpClient httpClient, ILocalStorageService localStorage, IServiceProvider sp)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri("https://localhost:7229/api/");
        HttpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        HttpClient.EnableIntercept(sp);

        _localStorage = localStorage;
    }

    public async Task<HttpResponseMessage> GetResponseAsync(string uriRequest)
    {
        HttpResponseMessage response = await HttpClient.GetAsync(uriRequest);
        return response;
    }

    public async Task<string> GetResponseAndContentAsync(string uriRequest)
    {
        HttpResponseMessage response = await HttpClient.GetAsync(uriRequest);
        var content = await response.Content.ReadAsStringAsync();
        if (CheckStatusResponse(response))
            return content;

        return content!;
    }

    public async Task<IEnumerable<T1>> GetAsync<T1>(JsonSerializerSettings options, string uriRequest)
    {
        //IMPORTANT : We don't need to add JWT header in the http client request as it is already handled by Interceptor Service
        //Please look into HttpInterceptorService class

        //string jwtToken = await GetJWTFromLocalStorage();
        //if (!string.IsNullOrEmpty(jwtToken))
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        HttpResponseMessage response = await HttpClient.GetAsync(uriRequest);
        var content = await response.Content.ReadAsStringAsync();
        if (CheckStatusResponse(response))
            return [];

        var result = JsonConvert.DeserializeObject<IEnumerable<T1>>(content, options);
        if (string.IsNullOrEmpty(content))
            return [];

        return result!;
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string uriRequest, T bodyContent)
    {
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(uriRequest, bodyContent);
        CheckStatusResponse(response);
        return response;
    }

    public void RemoveAuthorizationHeader()
    {
        HttpClient.DefaultRequestHeaders.Authorization = null;
    }

    private async Task<string> GetJWTFromLocalStorage()
    {
        //Get token from local storage
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (string.IsNullOrWhiteSpace(token))
            return string.Empty;
        else
            return token!;
    }

    public bool CheckStatusResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            //We use this as an alternative, because we still confuse either to return empty data as not found status response or success with empty value
            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
                return false;

            //response.EnsureSuccessStatusCode();
            //string? content = await response.Content.ReadAsStringAsync();
            throw new ApplicationException(response.ReasonPhrase);
        }

        return true;
    }
}
