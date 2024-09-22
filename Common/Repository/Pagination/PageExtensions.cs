using Common.Helpers.ZorroTableFilter;
using Microsoft.EntityFrameworkCore;

namespace Common.Repository.Pagination;

public static class PageExtensions
{
    public static async Task<PageList<T>> ToPageAsync<T>(
        this IQueryable<T> query,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageIndex < 1) throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page numbers start from 1");
        int startIndex = (pageIndex - 1) * pageSize;
        int count = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await query.Skip(startIndex)
                               .Take(pageSize)
                               .ToListAsync(cancellationToken)
                               .ConfigureAwait(false);
        return new PageList<T>(items, pageIndex, pageSize, count);
    }


    public static PageList<T> ToPage<T>(
        this IQueryable<T> query,
        int pageIndex,
        int pageSize)
    {
        if (pageIndex < 1) throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page numbers start from 1");
        int startIndex = (pageIndex - 1) * pageSize;
        int count = query.Count();
        var items = query.Skip(startIndex)
                         .Take(pageSize)
                         .ToList();
        return new PageList<T>(items, pageIndex, pageSize, count);
    }

    /// <summary>
    /// ng zorro library for front
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="zorroFilterRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static async Task<PageList<T>> ToPageZorroAsync<T>(
    this IQueryable<T> query,
    ZorroFilterRequest zorroFilterRequest,
    CancellationToken cancellationToken = default)
    {

        var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        var (items,filteredCount) = await query.ApplyPageRequestWithFilteredCount(zorroFilterRequest);


        var result = await items.ToListAsync(cancellationToken)
                                       .ConfigureAwait(false);
        return new PageList<T>(result, zorroFilterRequest.PageIndex, zorroFilterRequest.PageSize, count, filteredCount);
    }

    public static PageList<T> ToPageZorro<T>(
        this IQueryable<T> query,
        ZorroFilterRequest zorroFilterRequest)
    {
        var count = query.Count();
        var items = query.ApplyPageRequest(zorroFilterRequest).ToList();

        return new PageList<T>(items, zorroFilterRequest.PageIndex, zorroFilterRequest.PageSize, count);
    }
}

