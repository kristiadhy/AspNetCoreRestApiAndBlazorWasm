//using Newtonsoft.Json;
//using System.Net;
//using System.Net.Http.Headers;

//namespace Extension.Services;

//public class ClientService
//{
//    public static async Task<string> GetJWTFromLocalStorage()
//    {
//        //Get token from local storage
//        var token = await _localStorage.GetItemAsync<string>("authToken");
//        if (string.IsNullOrWhiteSpace(token))
//            return string.Empty;
//        else
//            return token!;
//    }

//    public static async Task<HttpClient> CreateDefaultClientService(IHttpClientFactory httpClientFactory)
//    {
//        HttpClient client = httpClientFactory.CreateClient("_");
//        string jwtToken = await GetJWTFromLocalStorage();
//        //Include the JWT token in the request.
//        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
//        return client;
//    }

//    public static async Task<List<T1>> ReadDataService<T1>(HttpClient client, JsonSerializerSettings options, string controllerName)
//    {
//        HttpResponseMessage response = await client.GetAsync($"{controllerName}");
//        //response.EnsureSuccessStatusCode();
//        var content = await response.Content.ReadAsStringAsync();
//        if (!response.IsSuccessStatusCode)
//        {
//            //We use this as an alternative, because we still confuse either to return empty data as not found status response or success with empty value
//            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
//                return [];

//            response.EnsureSuccessStatusCode();
//        }

//        var result = JsonConvert.DeserializeObject<List<T1>>(content, options);
//        if (string.IsNullOrEmpty(content))
//            return [];

//        return result!;
//    }
//}
