using Common.Extensions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Common.ResponseResult;

namespace Common.HttpClient;

public class BaseClient : IBaseClient
{
    public IHttpClientFactory HttpClient { get; set; }

    public BaseClient(IHttpClientFactory httpClient)
    {
        HttpClient = httpClient;
    }

    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = HttpClient.CreateClient("ApiClient");
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            client.DefaultRequestHeaders.Clear();
            if (apiRequest.Data != null)
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);

            message.Method = apiRequest.ApiType switch
            {
                ApiType.Post => HttpMethod.Post,
                ApiType.Put => HttpMethod.Put,
                ApiType.Delete => HttpMethod.Delete,
                _ => HttpMethod.Get
            };

            var apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var apiResponseData = JsonConvert.DeserializeObject<ResponseData<T>>(apiContent);
            var apiDto = default(T);
            if (apiResponseData != null)
                apiDto = apiResponseData.Result;

            return apiDto;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.GetMessage());
        }
    }

    public void Dispose() =>
        GC.SuppressFinalize(true);
}

