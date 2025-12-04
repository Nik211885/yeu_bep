namespace YeuBep.Helpers.ObjectFilter;

public class DataTypeAttribute : Attribute
{
    public List<Type> DateType { get; set; }
    public DataTypeAttribute(params Type[] dateType)
    {
        DateType = dateType.ToList();
    }
}