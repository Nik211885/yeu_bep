namespace YeuBep.ViewModels.Errors;

public class UnauthorizedViewModel
{
    public string Message { get; set; } = "Bạn không có quyền truy cập vào trang này.";
    public string RequestedPath { get; set; } = "Không xác định";
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}