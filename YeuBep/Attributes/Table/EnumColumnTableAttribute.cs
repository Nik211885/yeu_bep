namespace YeuBep.Attributes.Table;
[AttributeUsage(AttributeTargets.Field)]
public class EnumColumnTableAttribute : Attribute
{
    public LabelLevel LabelLevel { get; }
    public string ColumnName { get; }

    public EnumColumnTableAttribute(LabelLevel labelLevel, string columnName)
    {
        LabelLevel = labelLevel;
        ColumnName = columnName;
    }
}


// color for label
public enum LabelLevel
{
    Primary,
    Danger,
    Warning,
}