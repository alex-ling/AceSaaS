using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Util;

namespace Acesoft
{
    public static class DateExtensions
    {
        public static string ToDateStr(this DateTime dt)
        {
            return dt.ToStr("yyyy-MM-dd");
        }

        public static string ToTimeStr(this DateTime dt)
        {
            return dt.ToStr("yyyy-MM-dd HH:mm");
        }

        public static string ToStr(this DateTime dt, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dt.ToString(format);
        }

        public static string ToY(this DateTime dt)
        {
            return dt.Year.ToString();
        }

        public static string ToYM(this DateTime dt)
        {
            return dt.ToString("yyyyMM");
        }

        public static string ToYMD(this DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

        public static string ToDHMSF(this DateTime dt)
        {
            return dt.ToString("ddHHmmssff");
        }

        public static int GetChinaWeek(this DateTime dt)
        {
            var w = (int)dt.DayOfWeek;
            return w > 0 ? (w - 1) : 6;
        }

        public static long ToUnix(this DateTime dt)
        {
            return DatetimeHelper.ToUnix(dt);
        }
    }
}