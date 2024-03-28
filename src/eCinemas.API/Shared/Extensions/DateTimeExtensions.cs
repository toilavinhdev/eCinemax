namespace eCinemas.API.Shared.Extensions;

public static class DateTimeExtensions
{
    public static DateTimeOffset OriginTime
    {
        get
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var date = new DateTime(1970, 1, 1, 0, 0, 0);
            return TimeZoneInfo.ConvertTimeToUtc(date, timezone);
        }
    }

    public static int GetMinutesFromOriginTime(this DateTimeOffset time) => (time - OriginTime).Minutes + 1;

    public static int GetDayFromOriginTime(this DateTimeOffset time) => (time - OriginTime).Days + 1;

    public static DateTimeOffset GetDateStartTime(this DateTimeOffset time) 
        => new(time.Year, time.Month, time.Day, 0, 0, 0, TimeSpan.FromHours(7));
    
    public static DateTimeOffset GetDateEndTime(this DateTimeOffset time)
        => new(time.Year, time.Month, time.Day, 23, 59, 59, TimeSpan.FromHours(7));
}