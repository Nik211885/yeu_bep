namespace YeuBep.Helpers;

public static class DateTimeHelper
{
    public static string ConvertDateTimeFormat(DateTime dateTime)
    {
        var now = DateTime.Now;
        var ts = now - dateTime;

        if (ts.TotalSeconds < 60)
        {
            return $"{Math.Floor(ts.TotalSeconds)} giây trước";
        }

        if (ts.TotalMinutes < 60)
        {
            return $"{Math.Floor(ts.TotalMinutes)} phút trước";
        }

        if (ts.TotalHours < 24)
        {
            return $"{Math.Floor(ts.TotalHours)} giờ trước";
        }

        if (ts.TotalDays < 7)
        {
            return $"{Math.Floor(ts.TotalDays)} ngày trước";
        }
        return dateTime.ToString("dd/MM/yyyy");
    }
    public static string ConvertDateTimeZoneFormat(DateTimeOffset dateTime)
    {
        var vnTime = dateTime.ToOffset(TimeSpan.FromHours(7));
        var now = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(7));
    
        var ts = now - vnTime;

        if (ts.TotalSeconds < 60)
        {
            return $"{Math.Floor(ts.TotalSeconds)} giây trước";
        }

        if (ts.TotalMinutes < 60)
        {
            return $"{Math.Floor(ts.TotalMinutes)} phút trước";
        }

        if (ts.TotalHours < 24)
        {
            return $"{Math.Floor(ts.TotalHours)} giờ trước";
        }

        if (ts.TotalDays < 7)
        {
            return $"{Math.Floor(ts.TotalDays)} ngày trước";
        }
        return vnTime.ToString("dd/MM/yyyy");
    }
}