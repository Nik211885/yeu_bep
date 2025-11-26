namespace YeuBep.ViewModels.Errors;

public class InternalServerViewModel
{
    public string Message { get; set; } = "Đã xảy ra lỗi máy chủ.";
    public string RequestedPath { get; set; } = "Không xác định";
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    public string? ErrorId { get; set; }  // dùng để log hoặc trace
}