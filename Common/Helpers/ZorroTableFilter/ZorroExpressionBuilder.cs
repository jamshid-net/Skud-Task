using Common.MultiLanguage;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;


namespace Common.Helpers.ZorroTableFilter;
internal static class ZorroExpressionBuilder
{
    public static Expression<Func<T, bool>> BuildPredicate<T>(ZorroFilterRequest pageRequest)
    {
        if (pageRequest.Filter == null || !pageRequest.Filter.Any())
        {
            return x => true;
        }

        ParameterExpression param = Expression.Parameter(typeof(T), "x");
        Expression? combined = pageRequest.Filter.Select(filter => BuildSinglePredicate<T>(param, filter))
                                                 .Aggregate<Expression, Expression>(null, (current, predicate)
                                                  => current == null ? predicate : Expression.AndAlso(current, predicate));

        return Expression.Lambda<Func<T, bool>>(combined!, param);
    }

    private static Expression BuildSinglePredicate<T>(ParameterExpression param, ZorroFilter filter)
    {
        if (filter.Key.Contains(".multiLang", StringComparison.OrdinalIgnoreCase))
        {
            return FilterForMultiLang(param, filter);
        }
        if (filter.Key.EndsWith(".from", StringComparison.OrdinalIgnoreCase) ||
            filter.Key.EndsWith(".to", StringComparison.OrdinalIgnoreCase))
        {
            return FilterDatesFromTo(param, filter);
        }
        MemberExpression member = Expression.Property(param, filter.Key);
        object filterValue = filter.Value;

        if (filterValue is JsonElement jsonElement)
        {
            filterValue = ConvertJsonElement(jsonElement, member.Type);
        }

        if (member.Type.IsEnum)
        {
            return FilterForEnum(filterValue, member);
        }

        if (member.Type == typeof(DateTime) || member.Type == typeof(DateTime?))
        {
            return FilterForDateTime(filterValue, member);
        }

        if (member.Type == typeof(DateOnly) || member.Type == typeof(DateOnly?))
        {
            return FilterForDateOnly(filterValue, member);
        }

        if (member.Type == typeof(string))
        {
            return FilterForString(filterValue, member);
        }

        if (IsNumericType(member.Type))
        {
            return FilterForNumerics(filterValue, member);
        }

        throw new NotSupportedException($"Filter value type {filterValue.GetType()} is not supported.");
    }

    private static Expression FilterForNumerics(object filterValue, MemberExpression member)
    {
        if (filterValue is IEnumerable<object> numericArray)
        {
            var genericListType = typeof(List<>).MakeGenericType(member.Type);
            var numberList = Activator.CreateInstance(genericListType);

            // Get the 'Add' method for List<T>
            var addMethod = genericListType.GetMethod("Add");

            // Populate the list with converted values
            foreach (var value in numericArray)
            {
                var convertedValue = Convert.ChangeType(value, member.Type);
                addMethod?.Invoke(numberList, [convertedValue]);
            }

            // Create an expression that checks if the property is contained in the list
            MethodInfo containsMethod = genericListType.GetMethod("Contains", [member.Type])!;
            var listConstant = Expression.Constant(numberList);
            return Expression.Call(listConstant, containsMethod, member);
        }
        ConstantExpression constant = Expression.Constant(filterValue, member.Type);
        return Expression.Equal(member, constant);
    }

    private static Expression FilterForString(object filterValue, MemberExpression member)
    {
        if (filterValue is IEnumerable<object> stringArray)
        {
            // Convert the array to a List<string> for Contains method
            var stringList = stringArray
                .Select(value => value is JValue jValue ?
                    jValue.ToString(CultureInfo.InvariantCulture).ToLower() :
                    value.ToString()?.ToLower()).ToList();

            // Create an expression that checks if the property is contained in the list
            MethodInfo listContainsMethod = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) })!;
            var stringConstantList = Expression.Constant(stringList);

            return Expression.Call(stringConstantList, listContainsMethod, Expression.Call(member, "ToLower", Type.EmptyTypes));
        }
        MethodInfo containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!;
        return Expression.Call(member, containsMethod, Expression.Constant(filterValue.ToString()?.ToLower(), typeof(string)));
    }

    private static Expression FilterForDateOnly(object filterValue, MemberExpression member)
    {
        if (filterValue is string strFilter)
        {
            DateOnly parsedDateOnly = DateOnly.ParseExact(strFilter, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime parsedDateTime = parsedDateOnly.ToDateTime(TimeOnly.MinValue);

            var isNullable = member.Type == typeof(DateOnly?);

            ConstantExpression constantDateTime = Expression.Constant(parsedDateTime, typeof(DateTime));

            Expression memberExpression = isNullable
                ? (Expression)Expression.Convert(member, typeof(DateTime))
                : Expression.Convert(member, typeof(DateTime));

            Expression dateTimeProperty = Expression.Property(memberExpression, "Date");

            var comparison = Expression.Equal(dateTimeProperty, constantDateTime);

            if (isNullable)
            {
                var hasValueProperty = Expression.Property(member, "HasValue");
                var valueProperty = Expression.Property(member, "Value");

                var nullCheck = Expression.NotEqual(hasValueProperty, Expression.Constant(false));
                var valueComparison = Expression.Equal(Expression.Property(valueProperty, "Date"), constantDateTime);

                comparison = Expression.AndAlso(nullCheck, valueComparison);
            }

            return comparison;
        }

        throw new ArgumentException("Filter value must be a string in 'dd.MM.yyyy' format.");
    }

    private static Expression FilterForDateTime(object filterValue, MemberExpression member)
    {
        if (filterValue is string strFilter)
        {
            DateTime parsedDateTime = DateTime.ParseExact(
                strFilter,
                ["dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy HH:mm", "dd.MM.yyyy HH", "dd.MM.yyyy"],
                null,
                DateTimeStyles.None);

            DateTime rangeStart;
            DateTime rangeEnd;

            if (parsedDateTime is { Hour: 0, Minute: 0, Second: 0 })
            {

                rangeStart = new DateTime(parsedDateTime.Year, parsedDateTime.Month, parsedDateTime.Day, 0, 0, 0);
                rangeEnd = rangeStart.AddDays(1).AddSeconds(-1); // End of the day
            }
            else
            {

                rangeStart = new DateTime(parsedDateTime.Year, parsedDateTime.Month, parsedDateTime.Day, parsedDateTime.Hour, parsedDateTime.Minute, 0);
                rangeEnd = rangeStart.AddSeconds(59); // End of the minute
            }

            var isNullable = member.Type == typeof(DateTime?);

            // Create ConstantExpressions for the start and end of the range
            ConstantExpression constantRangeStart = Expression.Constant(rangeStart, typeof(DateTime));
            ConstantExpression constantRangeEnd = Expression.Constant(rangeEnd, typeof(DateTime));

            // Convert the member to DateTime if it's nullable
            Expression memberExpression = isNullable
                ? Expression.Convert(member, typeof(DateTime))
                : member;

            // Create expressions for comparing member with range start and end
            var greaterThanOrEqual = Expression.GreaterThanOrEqual(memberExpression, constantRangeStart);
            var lessThanOrEqual = Expression.LessThanOrEqual(memberExpression, constantRangeEnd);

            // Combine the two conditions with an AndAlso
            var rangeComparison = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

            // If the member is nullable, handle null cases
            if (isNullable)
            {
                var hasValueProperty = Expression.Property(member, "HasValue");
                var valueProperty = Expression.Property(member, "Value");

                var nullCheck = Expression.NotEqual(hasValueProperty, Expression.Constant(false));
                var valueComparison = Expression.AndAlso(
                    Expression.GreaterThanOrEqual(valueProperty, constantRangeStart),
                    Expression.LessThanOrEqual(valueProperty, constantRangeEnd)
                );

                rangeComparison = Expression.AndAlso(nullCheck, valueComparison);
            }

            return rangeComparison;
        }
        else
        {
            throw new ArgumentException("Filter value must be a string in valid date formats.");
        }
    }

    private static Expression FilterForEnum(object filterValue, MemberExpression member)
    {
        if (filterValue is IEnumerable<object> enumerable)
        {
            var enumValues = enumerable
                .Select(value => Enum.ToObject(member.Type, Convert.ChangeType(value, Enum.GetUnderlyingType(member.Type))))
                .ToArray();

            // Create a typed array of enum values
            var enumTypedArray = Array.CreateInstance(member.Type, enumValues.Length);
            Array.Copy(enumValues, enumTypedArray, enumValues.Length);

            // Build 'Contains' expression for enums
            MethodInfo containsMethod = typeof(Enumerable).GetMethods()
                .Single(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .MakeGenericMethod(member.Type);

            return Expression.Call(null, containsMethod, Expression.Constant(enumTypedArray), member);
        }
        // Convert the filter value to the corresponding enum type
        filterValue = Enum.ToObject(member.Type, filterValue);
        ConstantExpression constantEnum = Expression.Constant(filterValue, member.Type);
        return Expression.Equal(member, constantEnum);
    }

    private static Expression FilterDatesFromTo(ParameterExpression param, ZorroFilter filter)
    {
        var propertyKey = filter.Key.Split('.')[0];
        var memberExpression = Expression.Property(param, propertyKey);

        // Check for DateOnly, DateOnly?, DateTime, or DateTime? types
        if (memberExpression.Type != typeof(DateOnly) && memberExpression.Type != typeof(DateOnly?) &&
            memberExpression.Type != typeof(DateTime) && memberExpression.Type != typeof(DateTime?))
        {
            throw new InvalidOperationException($"Property '{propertyKey}' is not of type DateOnly, DateOnly?, DateTime, or DateTime?.");
        }

        ConstantExpression constantExpression = DateTimeAndDateOnlyExpression(memberExpression.Type, filter.Value);

        return filter.Key.EndsWith(".from", StringComparison.OrdinalIgnoreCase) ?
            // Create expression for GreaterThanOrEqual
            Expression.GreaterThanOrEqual(memberExpression, constantExpression) :
            // .to case
            // Create expression for LessThanOrEqual
            Expression.LessThanOrEqual(memberExpression, constantExpression);
    }

    private static Expression FilterForMultiLang(ParameterExpression param, ZorroFilter filter)
    {
        var propertyKey = filter.Key.Split('.')[0];

        var mainProperty = Expression.Property(param, propertyKey);

        if (mainProperty.Type != typeof(MultiLanguageField))
        {
            throw new InvalidOperationException($"Property '{propertyKey}' is not of type MultiLanguageField.");
        }


        var multiLanguageProperties = typeof(MultiLanguageField).GetProperties();
        Expression combinedOrExpression = (from property in multiLanguageProperties
                                           select Expression.Property(mainProperty, property.Name)
                into jsonPropertyAccess
                                           let toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!
                                           select Expression.Call(jsonPropertyAccess, toLowerMethod)
                into lowerProperty
                                           let exConstant = Expression.Constant(filter.Value.ToString()?.ToLower(), typeof(string))
                                           let containsMethod = typeof(string).GetMethod("Contains", [typeof(string)])!
                                           select Expression.Call(lowerProperty, containsMethod, exConstant))
            .Aggregate<MethodCallExpression, Expression>(null, (current, containsExpression) => current == null
                ? containsExpression
                : Expression.OrElse(current, containsExpression));


        return combinedOrExpression!;
    }

    private static object ConvertJsonElement(JsonElement jsonElement, Type targetType)
    {
        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        return jsonElement.ValueKind switch
        {
            JsonValueKind.String => underlyingType == typeof(DateOnly)
                ? DateOnly.Parse(jsonElement.GetString()!)
                : jsonElement.GetString()!,
            JsonValueKind.Number => Type.GetTypeCode(underlyingType) switch
            {
                TypeCode.Int32 => jsonElement.GetInt32(),
                TypeCode.Double => jsonElement.GetDouble(),
                TypeCode.Single => jsonElement.GetSingle(),
                TypeCode.Decimal => jsonElement.GetDecimal(),
                _ => throw new InvalidOperationException($"Cannot convert JSON number to {targetType}."),
            },
            JsonValueKind.True or JsonValueKind.False => jsonElement.GetBoolean(),
            JsonValueKind.Null when Nullable.GetUnderlyingType(targetType) != null => null,
            _ => throw new NotSupportedException($"JSON value kind {jsonElement.ValueKind} is not supported."),
        };
    }

    private static bool IsNumericType(Type type)
    {
        return type == typeof(int) || type == typeof(double) || type == typeof(float) || type == typeof(decimal) || type == typeof(int?) || type == typeof(double?);
    }


    private static ConstantExpression DateTimeAndDateOnlyExpression(Type memberType, object? filterValue)
    {
        var constant = filterValue switch
        {
            string dateString when memberType == typeof(DateOnly) || memberType == typeof(DateOnly?) =>
                DateOnly.TryParse(dateString, out var date)
                    ? Expression.Constant(date, memberType)
                    : throw new InvalidOperationException($"Cannot convert filter value '{filterValue}' to DateOnly."),

            string dateString when DateTime.TryParse(dateString, out var dateTime) =>
                Expression.Constant(dateTime, memberType),

            string dateString =>
                throw new InvalidOperationException($"Cannot convert filter value '{filterValue}' to DateTime."),

            DateOnly dateOnlyValue =>
                Expression.Constant(dateOnlyValue, memberType),

            DateTime dateTimeValue =>
                Expression.Constant(dateTimeValue, memberType),

            _ => throw new InvalidOperationException($"Cannot convert filter value '{filterValue}' to a supported date type.")
        };

        return constant;
    }


}