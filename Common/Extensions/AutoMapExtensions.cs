using AutoMapper;
using Common.Repository;
using Common.Repository.Pagination;

namespace Common.Extensions;

/// <summary>
///  
/// </summary>
public static class AutoMapExtensions
{
    public static void CommonMapping(this Profile profile)
    {
        //automap pageList
        profile.CreateMap(typeof(PageList<>), typeof(PageList<>)).ConvertUsing(typeof(PageListConverter<,>));

        profile.CreateMap<string, int>().ConvertUsing<IntTypeConverter>();
        profile.CreateMap<string, int?>().ConvertUsing<NullIntTypeConverter>();
        profile.CreateMap<string, decimal>().ConvertUsing<DecimalTypeConverter>();
        profile.CreateMap<string, decimal?>().ConvertUsing<NullDecimalTypeConverter>();

    }

    //AutoMapper PageList 
    private class PageListConverter<TSource, TDestination> : ITypeConverter<PageList<TSource>, PageList<TDestination>>
    {
        public PageList<TDestination> Convert(PageList<TSource> source, PageList<TDestination> destination, ResolutionContext context)
        {
            var mappedItems = context.Mapper.Map<List<TDestination>>(source.Items);
            return new PageList<TDestination>(mappedItems, source.PageIndex, source.PageSize, source.Total);
        }
    }

    #region AutoMapTypeConverters
    // Automap type converter definitions for 
    // int, int?, decimal, decimal?, bool, bool?, Int64, Int64?, DateTime
    // Automapper string to int?
    private class NullIntTypeConverter : ITypeConverter<string, int?>
    {
        public int? Convert(string source, int? destination, ResolutionContext context)
        {
            if (source == null)
                return null;
            else
            {
                int result;
                return int.TryParse(source, out result) ? result : null;
            }
        }
    }
    // Automapper string to int
    private class IntTypeConverter : ITypeConverter<string, int>
    {
        public int Convert(string source, int destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source))
                return 0;
            else
                return int.Parse(source);
        }
    }
    //// Automapper string to decimal?
    private class NullDecimalTypeConverter : ITypeConverter<string, decimal?>
    {
        public decimal? Convert(string source, decimal? destination, ResolutionContext context)
        {
            if (source == null)
                return null;
            else
            {
                decimal result;
                return decimal.TryParse(source, out result) ? result : null;
            }
        }
    }

    //// Automapper string to decimal
    private class DecimalTypeConverter : ITypeConverter<string, decimal>
    {
        public decimal Convert(string source, decimal destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source))
                return 0;
            else
                return Decimal.Parse(source);
        }
    }

    //// Automapper string to bool?
    //private class NullBooleanTypeConverter : TypeConverter<string, bool?>
    //{
    //    protected override bool? ConvertCore(string source)
    //    {
    //        if (source == null)
    //            return null;
    //        else
    //        {
    //            bool result;
    //            return Boolean.TryParse(source, out result) ? (bool?)result : null;
    //        }
    //    }
    //}
    //// Automapper string to bool
    //private class BooleanTypeConverter : TypeConverter<string, bool>
    //{
    //    protected override bool ConvertCore(string source)
    //    {
    //        if (source == null)
    //            throw new MappingException("null string value cannot convert to non-nullable return type.");
    //        else
    //            return Boolean.Parse(source);
    //    }
    //}
    //// Automapper string to Int64?
    //private class NullInt64TypeConverter : TypeConverter<string, Int64?>
    //{
    //    protected override Int64? ConvertCore(string source)
    //    {
    //        if (source == null)
    //            return null;
    //        else
    //        {
    //            Int64 result;
    //            return Int64.TryParse(source, out result) ? (Int64?)result : null;
    //        }
    //    }
    //}
    //// Automapper string to Int64
    //private class Int64TypeConverter : TypeConverter<string, Int64>
    //{
    //    protected override Int64 ConvertCore(string source)
    //    {
    //        if (source == null)
    //            throw new MappingException("null string value cannot convert to non-nullable return type.");
    //        else
    //            return Int64.Parse(source);
    //    }
    //}
    //// Automapper string to DateTime?
    //// In our case, the datetime will be a JSON2.org datetime
    //// Example: "/Date(1288296203190)/"
    //private class NullDateTimeTypeConverter : TypeConverter<string, DateTime?>
    //{
    //    protected override DateTime? ConvertCore(string source)
    //    {
    //        if (source == null)
    //            return null;
    //        else
    //        {
    //            DateTime result;
    //            return DateTime.TryParse(source, out result) ? (DateTime?)result : null;
    //        }
    //    }
    //}
    //// Automapper string to DateTime
    //private class DateTimeTypeConverter : TypeConverter<string, DateTime>
    //{
    //    protected override DateTime ConvertCore(string source)
    //    {
    //        if (source == null)
    //            throw new MappingException("null string value cannot convert to non-nullable return type.");
    //        else
    //            return DateTime.Parse(source);
    //    }
    //}
    #endregion
}

