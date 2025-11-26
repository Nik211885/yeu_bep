using System.Reflection;
using YeuBep.Attributes.Table;

namespace YeuBep.Extensions;

public static class PropertiesInfoExtensions
{
    extension(PropertyInfo prop)
    {
        public string? GetNameColumn()
        {
            var attr = prop.GetCustomAttribute<NameColumnAttribute>();
            return attr?.NameColumn;
        }

        public bool IsIgnoreColumn()
        {
            var attr = prop.GetCustomAttribute<IgnoreColumnAttribute>();
            if (attr != null)
            {
                return true;
            }

            return false;
        }
    }
}