namespace YeuBep.Attributes.Table;
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NameColumnAttribute : Attribute
{
    public string NameColumn { get; }
    public bool IsAdvancedSearch { get; set; } = false;
    public NameColumnAttribute(string nameColumn ,bool isAdvancedSearch = false)
    {
        NameColumn = nameColumn;
        IsAdvancedSearch = isAdvancedSearch;
    }
}