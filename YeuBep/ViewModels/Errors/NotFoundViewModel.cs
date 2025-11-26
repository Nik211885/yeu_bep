namespace YeuBep.ViewModels.Errors;

public class NotFoundViewModel
{
    public string Message { get; set; } = "Trang bạn tìm không tồn tại.";
    public string RequestedPath { get; set; } =  "Không xác định";
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}