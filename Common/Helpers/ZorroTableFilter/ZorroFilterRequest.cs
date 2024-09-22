namespace Common.Helpers.ZorroTableFilter;
public class ZorroFilterRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<ZorroSort>? Sort { get; set; }
    public List<ZorroFilter>? Filter { get; set; }
}

public class ZorroSort
{
    public string Key { get; set; }
    public ZorroSortEnum Value { get; set; }
}

public enum ZorroSortEnum
{
    /// <summary>
    /// 0 Ascanding like Orderby
    /// </summary>
    Asc = 0,

    /// <summary>
    /// 1 Descanding like OrderByDescanding
    /// </summary>
    Desc = 1,
}

public class ZorroFilter
{
    public string Key { get; set; }
    public object Value { get; set; }
}