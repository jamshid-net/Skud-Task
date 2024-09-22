using Newtonsoft.Json;

namespace Common.ResponseResult;

public class ResponseError
{
    [JsonProperty("code")]
    public int Code { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
    public ResponseError() { }
    public ResponseError(int code, string message)
    {
        Code = code;
        Message = message;
    }
}

