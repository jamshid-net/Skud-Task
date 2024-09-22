﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Helpers.ZorroTableFilter;
internal static class ZorroQueryableExtension
{
    public static IQueryable<T> ApplyPageRequest<T>(this IQueryable<T> query, ZorroFilterRequest pageRequest)
    {
        var predicate = ZorroExpressionBuilder.BuildPredicate<T>(pageRequest);
        query = query.Where(predicate);

        // Apply sorting if required
        if (pageRequest.Sort != null && pageRequest.Sort.Any())
        {
            var sortedQuery = pageRequest.Sort.Aggregate(query, ApplySorting);
            query = sortedQuery;
        }

        // Apply paging
        query = query.Skip(pageRequest.PageIndex * pageRequest.PageSize)
                     .Take(pageRequest.PageSize);

        return query;
    }
    public static async Task<(IQueryable<T>,int)> ApplyPageRequestWithFilteredCount<T>(this IQueryable<T> query, ZorroFilterRequest pageRequest)
    {
        var predicate = ZorroExpressionBuilder.BuildPredicate<T>(pageRequest);
        query = query.Where(predicate);

        // Apply sorting if required
        if (pageRequest.Sort != null && pageRequest.Sort.Any())
        {
            var sortedQuery = pageRequest.Sort.Aggregate(query, ApplySorting);
            query = sortedQuery;
        }
        int filteredCount = await query.CountAsync();
        // Apply paging
        query = query.Skip(pageRequest.PageIndex * pageRequest.PageSize)
            .Take(pageRequest.PageSize);

        return (query,filteredCount);
    }
    private static IOrderedQueryable<T> ApplySorting<T>(IQueryable<T> query, ZorroSort sort)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        MemberExpression property;

        if (sort.Key.Contains(".multiLanguage.", StringComparison.OrdinalIgnoreCase))
        {
            // Split the key to get the actual property and the language code (e.g., "Name" and "uz")
            var keyParts = sort.Key.Split([".multiLanguage."], StringSplitOptions.None);
            var propertyName = keyParts[0];
            var languageCode = keyParts[1];

            // Get the main property (e.g., Name)
            var multiLanguageFieldProperty = typeof(T).GetProperty(propertyName);
            if (multiLanguageFieldProperty == null)
            {
                throw new InvalidOperationException($"Property {propertyName} not found on type {typeof(T).Name}");
            }

            // Get the language-specific property within the MultiLanguageField (e.g., "uz")
            var multiLanguageFieldExpression = Expression.Property(parameter, multiLanguageFieldProperty);
            property = Expression.Property(multiLanguageFieldExpression, languageCode);
        }

        else
        {
            // Standard sorting for non-MultiLanguageField properties
            property = Expression.Property(parameter, sort.Key);
        }
        var lambda = Expression.Lambda(property, parameter);

        var methodName = sort.Value switch
        {
            ZorroSortEnum.Asc => "OrderBy",
            ZorroSortEnum.Desc => "OrderByDescending",
            _ => "OrderBy"
        };

        var method = typeof(Queryable)
            .GetMethods()
            .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type);

        return (IOrderedQueryable<T>)method.Invoke(null, [query, lambda])!;
    }
}
