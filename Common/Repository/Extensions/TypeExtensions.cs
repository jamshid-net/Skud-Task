using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace Common.Repository.Extensions;

public static class TypeExtensions
{
    public static List<Assembly> GetCurrentPathAssembly(this AppDomain domain)
    {
        var dlls = DependencyContext.Default!.CompileLibraries
            .Where(x => !x.Name.StartsWith("Microsoft") && !x.Name.StartsWith("System"))
            .ToList();
        var list = new List<Assembly>();
        if (dlls.Any())
        {
            list.AddRange(from dll in dlls where dll.Type == "project" select Assembly.Load(dll.Name));
        }
        return list;
    }

    public static bool HasImplementedRawGeneric(this Type type, Type generic)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (generic == null) throw new ArgumentNullException(nameof(generic));
        var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
        if (isTheRawGenericType) return true;
        while (type != null && type != typeof(object))
        {
            isTheRawGenericType = IsTheRawGenericType(type);
            if (isTheRawGenericType) return true;
            type = type.BaseType;
        }
        return false;

        bool IsTheRawGenericType(Type test)
            => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
    }
}

