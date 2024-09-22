using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Common.MultiLanguage;

[Owned]
public class MultiLanguageField
{
    [JsonPropertyName("uz")]
    public string Uz { get; set; }

    [JsonPropertyName("ru")]
    public string Ru { get; set; }
}

