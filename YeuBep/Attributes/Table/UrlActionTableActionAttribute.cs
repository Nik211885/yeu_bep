namespace YeuBep.Attributes.Table;
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