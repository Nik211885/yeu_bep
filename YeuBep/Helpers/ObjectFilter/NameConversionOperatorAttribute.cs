namespace YeuBep.Helpers.ObjectFilter;

public class NameConversionOperatorAttribute : Attribute
{
    public string NameConversion { get; set; }

    public NameConversionOperatorAttribute(string nameConversion)
    {
        NameConversion = nameConversion;
    }
}