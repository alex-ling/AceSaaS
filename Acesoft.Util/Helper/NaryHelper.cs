using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Util
{
    public static class NaryHelper
    {
        const string CHR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

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


        /// <summary>
        /// 16进制转为数字，最后2位为小数部分
        /// </summary>
        public static decimal HexToDecimal(string str)
        {
            return decimal.Parse($"{HexToInt(str, 0, str.Length - 2)}.{HexToInt(str, 2, 2)}");
        }

        /// <summary>
        /// 16进制字符串转为32位整数
        /// </summary>
        public static int HexToInt(string str)
        {
            return (int)ToNary(str, 16);
        }

        /// <summary>
        /// 16进制字符串转为32位整数
        /// </summary>
        public static int HexToInt(string str, int start, int length)
        {
            return (int)ToNary(str.Substring(start, length), 16);
        }

        /// <summary>
        /// 数据转16进制指定长度字符串，左侧补字符'0'
        /// </summary>
        public static string ToHex(this int value, int length)
        {
            var str = FromNary(value, 16);
            if (str.Length > length)
            {
                throw new AceException($"值{value}]转换后大于给定的长度{length}]");
            }

            return str.PadLeft(length, '0');
        }
    }
}
