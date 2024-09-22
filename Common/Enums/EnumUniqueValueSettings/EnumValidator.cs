using System.Reflection;

namespace Common.Enums.EnumUniqueValueSettings;
public static class EnumValidator
{
    public static void ValidateEnumValues(Assembly assembly)
    {
       
        var enumTypes = assembly.GetTypes()
            .Where(t => t.IsEnum && t.GetCustomAttribute<UniqueEnumValuesAttribute>() != null);

        foreach (var enumType in enumTypes)
        {
            var enumValues = Enum.GetValues(enumType).Cast<int>().ToList();

            var duplicates = enumValues.GroupBy(v => v)
                .Where(g => g.Count() > 1)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .ToList();

            if (duplicates.Count > 0)
            {
                throw new InvalidOperationException($"Enum {enumType.Name} has duplicate values: {string.Join(", ", duplicates.Select(d => $"Value {d.Value} is repeated {d.Count} times"))}");
            }
        }
    }
}
