namespace YeuBep.Helpers.ObjectFilter;

public enum OperatorForDataType
{
    [NameConversionOperator("Bằng")]
    [DataType(typeof(string), typeof(int), typeof(double), typeof(long), typeof(decimal),
        typeof(DateTime), typeof(DateTimeOffset), typeof(Enum))]
    Equals,
    [NameConversionOperator("Không bằng")]
    [DataType(typeof(string), typeof(int), typeof(double), typeof(long), typeof(decimal),
        typeof(DateTime), typeof(DateTimeOffset), typeof(Enum))]
    NotEquals,
    [NameConversionOperator("Lớn hơn hoặc bằng")]
    [DataType(typeof(int), typeof(double), typeof(long), typeof(decimal),
        typeof(DateTime), typeof(DateTimeOffset))]
    GreaterThanOrEqual,
    [NameConversionOperator("Nhỏ hơn hoặc bằng")]
    [DataType(typeof(int), typeof(double), typeof(long), typeof(decimal),
        typeof(DateTime), typeof(DateTimeOffset))]
    LessThanOrEqual,
    [NameConversionOperator("Bắt đầu bằng")]
    [DataType(typeof(string))]
    StartWith,
    [NameConversionOperator("Kết thúc bằng")]
    [DataType(typeof(string))]
    EndWith,
    [NameConversionOperator("Chứa")]
    [DataType(typeof(string))]
    Contains
}