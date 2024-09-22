using Newtonsoft.Json;

namespace Common.ResponseResult;

public class ResponseList
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

}

