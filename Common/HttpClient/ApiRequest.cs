namespace Common.HttpClient;

public class ApiRequest
{
    public ApiType ApiType { get; set; } = ApiType.Get;
    public string Url { get; set; }
    public object Data { get; set; }
    public string AccessToken { get; set; }
}

