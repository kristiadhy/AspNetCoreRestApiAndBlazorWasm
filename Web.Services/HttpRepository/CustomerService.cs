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
    private readonly DefaultApiService _client;
    private readonly JsonSerializerSettings _options;
    private readonly string controllerName = "customers";

    public CustomerService(DefaultApiService client, JsonSerializerSettings options)
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

        var queryHelper = QueryHelpers.AddQueryString(controllerName, queryStringParam!);
        HttpResponseMessage response = await _client.GetResponseAsync(queryHelper);
        var content = await response.Content.ReadAsStringAsync();
        _client.CheckStatusResponse(response);

        var pagingResponse = new PagingResponse<CustomerDTO>()
        {
            Items = JsonConvert.DeserializeObject<List<CustomerDTO>>(content, _options)!,
            MetaData = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)!
        };

        return pagingResponse;
    }

    public async Task<CustomerDTO> GetCustomerByID(Guid customerID)
    {
        var content = await _client.GetResponseAndContentAsync($"{controllerName}/{customerID}");
        var result = JsonConvert.DeserializeObject<CustomerDTO>(content, _options);
        if (!string.IsNullOrEmpty(content))
            return result!;
        else
            return new();
    }

    public async Task<HttpResponseMessage> Create(CustomerDTO customerDTO)
    {
        var postResult = await _client.PostAsync(controllerName, customerDTO);
        return postResult;
    }

    public async Task<HttpResponseMessage> Update(CustomerDTO customerDTO)
    {
        var postResult = await _client.PutAsync(controllerName, customerDTO);
        return postResult;
    }

    public Task<HttpResponseMessage> Delete(List<CustomerDTO> customerDTO)
    {
        throw new NotImplementedException();
    }
}
