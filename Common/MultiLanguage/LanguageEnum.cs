using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Common.MultiLanguage;

[JsonConverter(typeof(StringEnumConverter))]
public enum LanguageEnum
{
    [EnumMember(Value = "uz")]
    Uz,
    [EnumMember(Value = "ru")]
    Ru,
    [EnumMember(Value = "en")]
    En
}

