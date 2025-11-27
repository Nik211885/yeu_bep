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

        private bool IsAttribute<TAttribute>() where TAttribute : Attribute
        {
            var attr = prop.GetCustomAttribute<TAttribute>();
            if (attr != null)
            {
                return true;
            }

            return false;
        }
        public bool IsIgnoreColumn()
        {
            return prop.IsAttribute<IgnoreColumnAttribute>();
        }
        public bool IsKeyTableColumn()
        {
            return prop.IsAttribute<KeyTable>();
        }
    }
}