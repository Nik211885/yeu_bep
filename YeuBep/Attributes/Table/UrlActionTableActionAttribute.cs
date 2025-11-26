namespace YeuBep.Attributes.Table;
[AttributeUsage(AttributeTargets.Field)]
public class UrlActionTableActionAttribute : Attribute
{
    public string Action { get; }
    public string Controller { get; }

    public UrlActionTableActionAttribute(string action, string controller)
    {
        Action = action;
        Controller = controller;
    }
}