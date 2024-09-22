using Newtonsoft.Json;
using System.Globalization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;


namespace Common.Helpers;

#region DateTimeConverters
public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string[] _dateFormats =
    [
        "dd.MM.yyyy HH:mm",
        "dd.MM.yyyy HH:mm:ss",
        "dd.MM.yyyy HH",
        "dd.MM.yyyy",
        "yyyy-MM-ddTHH:mm:ss.fffZ",
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-ddTHH:mm:ss"
    ];

    private const string DateFormat = "dd.MM.yyyy HH:mm";

    public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Null:
                throw new JsonSerializationException("Cannot convert null value to DateTime.");
            case JsonToken.Date:
                return (DateTime)reader.Value!;
            case JsonToken.String:
            {
                var dateString = reader.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(dateString))
                {
                    if (DateTime.TryParseExact(dateString, _dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    {
                        return date;
                    }
                    throw new JsonSerializationException($"Unable to parse '{dateString}' to DateTime using formats: {string.Join(", ", _dateFormats)}.");
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }

        throw new JsonSerializationException($"Unexpected token parsing date. Expected String or Date, got {reader.TokenType}.");
    }

    public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}

public class CustomDateTimeNullableConverter : JsonConverter<DateTime?>
{
    private readonly string[] _dateFormats =
    [
        "dd.MM.yyyy HH:mm",
        "dd.MM.yyyy HH:mm:ss",
        "dd.MM.yyyy HH",
        "dd.MM.yyyy",
        "yyyy-MM-ddTHH:mm:ss.fffZ",
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-ddTHH:mm:ss"
    ];

    private const string DateFormat = "dd.MM.yyyy HH:mm";

    public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.Null:
                throw new JsonSerializationException("Cannot convert null value to DateTime.");
            case JsonToken.Date:
                return (DateTime)reader.Value!;
            case JsonToken.String:
            {
                var dateString = reader.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(dateString))
                {
                    if (DateTime.TryParseExact(dateString, _dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    {
                        return date;
                    }
                    throw new JsonSerializationException($"Unable to parse '{dateString}' to DateTime using formats: {string.Join(", ", _dateFormats)}.");
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }

        throw new JsonSerializationException($"Unexpected token parsing date. Expected String or Date, got {reader.TokenType}.");
    }

    public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}
#endregion

#region DateOnlyConverters
public class CustomDateOnlyConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "dd.MM.yyyy";

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String)
            throw new JsonSerializationException($"Invalid DateOnly format. Expected {DateFormat}.");

        var dateString = reader.Value?.ToString();
        if (DateOnly.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
        {
            return date;
        }

        throw new JsonSerializationException($"Invalid DateOnly format. Expected {DateFormat}.");
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}
public class CustomDateOnlyNullableConverter : JsonConverter<DateOnly?>
{
    private const string DateFormat = "dd.MM.yyyy";

    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String)
            throw new JsonSerializationException($"Invalid DateOnly format. Expected {DateFormat}.");

        var dateString = reader.Value?.ToString();
        if (DateOnly.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
        {
            return date;
        }

        throw new JsonSerializationException($"Invalid DateOnly format. Expected {DateFormat}.");
    }

    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}
#endregion