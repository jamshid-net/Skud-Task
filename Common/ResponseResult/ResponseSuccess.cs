using Newtonsoft.Json;

namespace Common.ResponseResult;

public class ResponseSuccess(bool isSuccess)
{
    [JsonProperty("success")]
    public bool Success { get; set; } = isSuccess;

    public ResponseSuccess() : this(true)
    {

    }
}

