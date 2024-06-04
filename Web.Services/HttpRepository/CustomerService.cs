using Domain.DTO;
using Domain.Parameters;
using Features;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Services.Extensions;
using Services.IRepositories;

namespace Services.Repositories;
public class CustomerService : ICustomerService
{
    private readonly CustomHttpClient _client;
    private readonly JsonSerializerSettings _options;
    private readonly string additionalResourceName = "customers";

    public CustomerService(CustomHttpClient client, JsonSerializerSettings options)
    {
        _client = client;
        _options = options;
    }

    public async Task<PagingResponse<CustomerDTO>> GetCustomers(CustomerParam customerParameter)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = customerParameter.PageNumber.ToString(),
            ["searchTerm"] = customerParameter.srcByName == null ? "" : customerParameter.srcByName,
            ["orderBy"] = customerParameter.OrderBy!
        };

        var queryHelper = QueryHelpers.AddQueryString(additionalResourceName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseForGetMethod(response);

        var pagingResponse = new PagingResponse<CustomerDTO>()
        {
            Items = JsonConvert.DeserializeObject<List<CustomerDTO>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)!
        };

        return pagingResponse;
    }

    public async Task<CustomerDTO> GetCustomerByID(Guid customerID)
    {
        var content = await _client.GetResponseAndContentAsync($"{additionalResourceName}/{customerID}");
        var result = JsonConvert.DeserializeObject<CustomerDTO>(content, _options);
        if (!string.IsNullOrEmpty(content))
            return result!;
        else
            return new();
    }

    public async Task<HttpResponseMessage> Create(CustomerDTO customerDTO)
    {
        var response = await _client.PostAsync(additionalResourceName, customerDTO);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseForPostMethod(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Update(CustomerDTO customerDTO)
    {
        var response = await _client.PutAsync(additionalResourceName, customerDTO);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseForPostMethod(response, content, _options);
        return response;
    }

    public async Task<HttpResponseMessage> Delete(Guid customerID)
    {
        var response = await _client.DeleteAsync($"{additionalResourceName}/{customerID}");
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckErrorResponseForPostMethod(response, content, _options);
        return response;
    }
}
