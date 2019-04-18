using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Util
{
    public static class DatetimeHelper
    {
        public static readonly DateTime ZERO = new DateTime(1970, 1, 1);
        const long L_ZERO = 621355968000000000L;
        const long L_MULT = 10000000L;
        const long L_DIFF = 28800L;

        public static DateTime ParseTodayTime(string time)
        {
            var items = time.Split(':');
            var now = DateTime.Now;
            var hour = items[0].ToObject<int>();
            var mins = items.Length > 1 ? items[1].ToObject<int>() : 0;
            return new DateTime(now.Year, now.Month, now.Day, hour, mins, 0);
        }

        public static long GetNowUnix()
        {
            return ToUnix(DateTime.Now);
        }

        public static string GetNowHex()
        {
            var now = DateTime.Now;
            return $"{now.Year.ToHex(4)}{now.Month.ToHex(2)}{now.Day.ToHex(2)}{now.Hour.ToHex(2)}{now.Minute.ToHex(2)}{(now.GetChinaWeek()+1).ToHex(2)}";
        }

        public static long GetNowMilliseconds()
        {
            return (long)(DateTime.UtcNow - ZERO.ToUniversalTime()).TotalMilliseconds;
        }

        public static long ToUnix(DateTime dt)
        {
            return (dt.Ticks - L_ZERO) / L_MULT - L_DIFF;
        }

        public static DateTime FromUnix(long ticks)
        {
            return ZERO.AddTicks((ticks + L_DIFF) * L_MULT);
        }

        public static string Replace(string str)
        {
            if (!str.HasValue())
            {
                return str;
            }
            var d = DateTime.Now;
            return str
                .Replace("{@date}", d.ToDateStr())
                .Replace("{@time}", d.ToTimeStr())
                .Replace("{@year}", d.ToY())
                .Replace("{@ym}", d.ToYM())
                .Replace("{@ymd}", d.ToYMD());
        }
    }
}
