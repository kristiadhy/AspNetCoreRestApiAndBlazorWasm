using Blazored.LocalStorage;
using Domain.DTO;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Web.Services.Features;

namespace Services.Extensions;
public class DefaultApiService
{
    //IMPORTANT : Here is the logic of how we use the httpclient service
    //1. We register httpclient as class/service and set all of the configuration here in the constructor.

    //IMPORTANT : We don't need to add JWT header in the http client request as it is already handled by Interceptor Service
    //Please look into HttpInterceptorService class

    private readonly HttpClient HttpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly WebHostEnvironment _hostEnvironment;

    public DefaultApiService(HttpClient httpClient, ILocalStorageService localStorage, IServiceProvider sp, WebHostEnvironment hostEnvironment)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri("https://localhost:7229/api/");
        HttpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        HttpClient.EnableIntercept(sp);

        _localStorage = localStorage;
        _hostEnvironment = hostEnvironment;
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
        CheckErrorResponseForGetMethod(response);

        return content!;
    }

    public async Task<IEnumerable<T1>> GetAsync<T1>(JsonSerializerSettings options, string uriRequest)
    {
        HttpResponseMessage response = await HttpClient.GetAsync(uriRequest);
        var content = await response.Content.ReadAsStringAsync();
        if (!CheckErrorResponseForGetMethod(response))
            return [];

        var result = JsonConvert.DeserializeObject<IEnumerable<T1>>(content, options);
        if (string.IsNullOrEmpty(content))
            return [];

        return result!;
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string uriRequest, T bodyContent)
    {
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(uriRequest, bodyContent);
        return response;
    }

    public async Task<HttpResponseMessage> PutAsync<T>(string uriRequest, T bodyContent)
    {
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync(uriRequest, bodyContent);
        return response;
    }

    public async Task<HttpResponseMessage> DeleteAsync(string additionalResourceName)
    {
        HttpResponseMessage response = await HttpClient.DeleteAsync(additionalResourceName);
        return response;
    }

    public void RemoveAuthorizationHeader()
    {
        HttpClient.DefaultRequestHeaders.Authorization = null;
    }

    public bool CheckErrorResponseForGetMethod(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            //We use this as an alternative, because we still confuse either to return empty data as not found status response or success with empty value
            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
                return false;

            //response.EnsureSuccessStatusCode();
            throw new ApplicationException($"{response.ReasonPhrase}");
        }

        return true;
    }

    public void CheckErrorResponseForPostMethod(HttpResponseMessage response, string content, JsonSerializerSettings options)
    {
        if (!response.IsSuccessStatusCode)
        {
            //If there is an error in the server, the server's middleware will return ResponseDto
            var serviceResponse = JsonConvert.DeserializeObject<ResponseDto>(content, options);
            //Show error detail if host environment mode is "Development"
            string errorResponse = $"{response.ReasonPhrase}";
            if (_hostEnvironment.IsDevelopment)
                errorResponse += $" - {serviceResponse?.Error}";

            throw new ApplicationException($"{errorResponse}");
        }
    }
}
