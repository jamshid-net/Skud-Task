namespace Common.HttpClient;

public interface IBaseClient : IDisposable
{
    Task<T> SendAsync<T>(ApiRequest apiRequest);
}

