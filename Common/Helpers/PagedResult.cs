namespace Common.Helpers;

using System.Collections.Generic;

public class PagedResult<T>
{
    public IList<T> Data { get; set; }

    public int Total { get; set; }
}


