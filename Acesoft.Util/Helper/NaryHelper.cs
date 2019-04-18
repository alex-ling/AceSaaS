using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Util
{
    public static class NaryHelper
    {
        const string CHR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        #region nary
        public static char ToChar(int t)
        {
            if (t > 62)
            {
                throw new AceException("最大为62进制（0-9A-Za-z）");
            }
            return CHR[t - 1];
        }

        public static long ToNary(string s, int t)
        {
            long n = 0;
            if (s.Length > 1)
            {
                n += t * ToNary(s.Substring(0, s.Length - 1), t) + ToNary(s.Substring(s.Length - 1), t);
            }
            else
            {
                char c = s[0];
                if (c > '9') n = c - 55;
                else n = long.Parse(s);
            }
            return n;
        }
        
        public static string FromNary(long n, int t)
        {
            string s = "";
            if (n == 0) return "0";
            while (n > 0)
            {
                var r = n % t;
                if (r <= 9) s = r.ToString() + s;
                else s = ((char)(r + 55)).ToString() + s;
                n = n / t;
            }
            return s;
        }
        #endregion

        /// <summary>
        /// 16进制原码转为数字，最后2位为小数部分
        /// </summary>
        public static double YmHexToDouble(string str)
        {
            return double.Parse($"{YmHexToInt(str.Substring(0, str.Length - 2))}.{HexToInt(str.Right(2))}");
        }

        /// <summary>
        /// 数字转为16进制原码表示
        /// </summary>
        public static string ToYmHex(this double value, int length)
        {
            return ToYmHex(value.ToString(), length);
        }

        /// <summary>
        /// 数字转为16进制原码表示
        /// </summary>
        public static string ToYmHex(string value, int length)
        { 
            var items = value.Split('.');
            var zsHex = int.Parse(items[0]).ToYmHex(length - 2);
            var xsHex = items.Length > 1 ? int.Parse(items[1]).ToHex(2) : "00";
            return $"{zsHex}{xsHex}";
        }

        /// <summary>
        /// 16进制字符串转为32位整数
        /// </summary>
        public static int HexToInt(string str)
        {
            //return (int)ToNary(str, 16);
            return Convert.ToInt32(str, 16);
        }

        /// <summary>
        /// 16进制字符串转为32位整数
        /// </summary>
        public static int HexToInt(string str, int start, int length)
        {
            return HexToInt(str.Substring(start, length));
        }

        /// <summary>
        /// Hex原码转数字
        /// </summary>
        public static int YmHexToInt(string str)
        {
            var power = (int)Math.Pow(2, 4 * str.Length - 1);
            var value = Convert.ToInt32(str, 16);
            if ((value & power) > 0)
            {
                return -(value & (power - 1));
            }
            return value;
        }

        /// <summary>
        /// 按原码方式转换
        /// </summary>
        public static string ToYmHex(this int value, int length)
        {
            var power = (int)Math.Pow(2, 4 * length - 1);
            var max = power - 1;
            Check.Require(-max <= value && value <= max, $"超过给定长度原码数范围：-{max}~{max}");

            if (value >= 0)
            {
                return value.ToString("X").PadLeft(length, '0');
            }
            else
            {
                return (-value | power).ToString("X").PadLeft(length, '0');
            }
        }

        /// <summary>
        /// 数据转16进制指定长度字符串，左侧补字符'0'
        /// </summary>
        public static string ToHex(this int value, int length)
        {
            //var str = FromNary(value, 16);
            var str = value.ToString("X");
            if (str.Length > length)
            {
                throw new AceException($"值{value}]转换后大于给定的长度{length}]");
            }

            return str.PadLeft(length, '0');
        }
    }
}
