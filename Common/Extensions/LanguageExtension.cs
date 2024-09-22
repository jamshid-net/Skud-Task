using Common.MultiLanguage;

namespace Common.Extensions;

public static class LanguageExtension
{
    public static string GetFieldValue(this MultiLanguageField field, LanguageEnum code)
    {
        return (string)typeof(MultiLanguageField).GetProperty(code.ToString())?.GetValue(field);
    }
}