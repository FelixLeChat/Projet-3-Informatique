using System;

namespace HttpHelper.Time
{
    public class TimeHelper
    {
        public static string CurrentCanadaTimeString()
        {
            return CurrentCanadaTime().ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DateTime CurrentCanadaTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(),
                                TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time")).AddHours(-1);
        }
    }
}
