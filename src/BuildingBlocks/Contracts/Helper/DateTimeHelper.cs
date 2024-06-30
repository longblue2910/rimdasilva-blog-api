namespace Contracts.Helper;

public static class DateTimeHelper
{
    public static string ConvertToAgoString(this DateTime dateTime)
    {
        TimeSpan timeAgo = DateTime.Now - dateTime;

        if (timeAgo.TotalSeconds < 60)
        {
            return $"{(int)timeAgo.TotalSeconds} giây trước";
        }
        else if (timeAgo.TotalMinutes < 60)
        {
            return $"{(int)timeAgo.TotalMinutes} phút trước";
        }
        else if (timeAgo.TotalHours < 24)
        {
            return $"{(int)timeAgo.TotalHours} giờ trước";
        }
        else if (timeAgo.TotalDays < 30)
        {
            return $"{(int)timeAgo.TotalDays} ngày trước";
        }
        else if (timeAgo.TotalDays < 365)
        {
            int months = (int)(timeAgo.TotalDays / 30);
            return $"{months} tháng trước";
        }
        else
        {
            int years = (int)(timeAgo.TotalDays / 365);
            return $"{years} năm trước";
        }
    }
}
