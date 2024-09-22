using System.ComponentModel;
using System.Reflection;

namespace Common.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumElement)
    {
        Type type = enumElement.GetType();
        MemberInfo[] memInfo = type.GetMember(enumElement.ToString());
        if (memInfo != null && memInfo.Length > 0)
        {
            object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
        }

        return enumElement.ToString();
    }
}

