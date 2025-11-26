namespace YeuBep.ViewModels;

public class ButtonConfig
{
    public ButtonTableType Type { get; set; }
    public string Controller { get; set; } = "";
    public string Action { get; set; } = "";
}
public enum ButtonTableType
{
    Add,
    Edit,
    Delete,
    Detail,
    Approve,
    UnApprove,
    Lock,
    Unlock,
}