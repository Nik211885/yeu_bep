namespace YeuBep.Attributes.Table;
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NameColumnAttribute : Attribute
{
    public string NameColumn { get; }
    public NameColumnAttribute(string nameColumn)
    {
        NameColumn = nameColumn;
    }
}