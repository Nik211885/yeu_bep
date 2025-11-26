using YeuBep.Attributes.Table;

namespace YeuBep.Extensions;

public static class EnumExtensions
{
    extension(Enum @enum)
    {
        public (LabelLevel, string)? GetLabelName()
        {
            var type = @enum.GetType();
            var member = type.GetMember(@enum.ToString()).FirstOrDefault();
            if (member == null) return null;
            var attr = member.GetCustomAttributes(typeof(EnumColumnTableAttribute), false)
                .Cast<EnumColumnTableAttribute>()
                .FirstOrDefault();
            if (attr == null) return null;
            return (attr.LabelLevel, attr.ColumnName);
        }
    }
}