using Newtonsoft.Json;

namespace Common.ResponseResult;

public class ResponseMessage(string message)
{
    [JsonProperty("message")]
    public string Message { get; set; } = message;

    public ResponseMessage() : this("")
    {
    }
}

