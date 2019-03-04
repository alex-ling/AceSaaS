using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft
{
    public static class StringExtensions
    {
        public static int GetBytesLength(this string str)
        {
            return Encoding.Default.GetByteCount(str);
        }

        public static string Left(this string str, int length)
        {
            if (!str.HasValue()) return "";
            return str.Substring(0, length);
        }

        public static string Right(this string str, int length)
        {
            if (!str.HasValue()) return "";
            return str.Substring(str.Length - length);
        }

        public static string LeftOfBytes(this string str, int len)
        {
            string result = string.Empty;
            if (!str.HasValue())
            {
                return result;
            }

            int byteLen = Encoding.Default.GetByteCount(str);
            int charLen = str.Length;
            int byteCount = 0;
            int pos = 0;

            if (byteLen > len)
            {
                var chars = str.ToCharArray();
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(chars[i]) > 255)
                    {
                        byteCount += 2;
                    }
                    else
                    {
                        byteCount += 1;
                    }
                    if (byteCount > len)
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)
                    {
                        pos = i + 1;
                        break;
                    }
                }
                if (pos >= 0)
                {
                    result = str.Substring(0, pos);
                }
            }
            else
            {
                result = str;
            }
            return result;
        }
        
        public static bool HasValue(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static string Conact(this string str, params object[] values)
        {
            return string.Concat(str, string.Concat(values));
        }

        public static string FormatWith(this string str, params object[] values)
        {
            return string.Format(str, values);
        }

        public static IList<T> Split<T>(this string values, char separator = ',')
        {
            var list = new List<T>();
            foreach (var value in values.Split(separator))
            {
                if (value.HasValue())
                {
                    list.Add(value.ToObject<T>());
                }
            }
            return list;
        }

        public static U ToObject<U>(this string value)
        {
            return (U)ToObject(value, typeof(U));
        }

        public static object ToObject(this string value, Type type)
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, value, true);
            }
            return Convert.ChangeType(value, type);
        }

        public static string ReplaceOnce(this string str, string find, string replace)
        {
            return ReplaceOnce(str, find, replace, StringComparison.CurrentCulture);
        }

        public static string ReplaceOnce(this string str, string find, string replace, StringComparison comp)
        {
            Check.Require(!string.IsNullOrEmpty(str), "源串不能为空");
            Check.Require(!string.IsNullOrEmpty(find), "匹配串不能为空");

            int firstIndex = str.IndexOf(find, comp);
            if (firstIndex != -1)
            {
                if (firstIndex == 0)
                {
                    str = replace + str.Substring(find.Length);
                }
                else if (firstIndex == (str.Length - find.Length))
                {
                    str = str.Substring(0, firstIndex) + replace;
                }
                else
                {
                    str = str.Substring(0, firstIndex) + replace + str.Substring(firstIndex + find.Length);
                }
            }
            return str;
        }

        public static string Replace(this string str, IDictionary<string, object> dict)
        {
            foreach (var p in dict)
            {
                str = str.Replace($"{{@{p.Key}}}", (p.Value ?? "").ToString());
            }
            return str;
        }

        public static StringBuilder Remove(this StringBuilder sb, int lentgh = 1, bool last = true)
        {
            Check.Require(lentgh <= sb.Length, "移除的长度不能超过总长度");

            return sb.Remove(last ? (sb.Length - lentgh) : 0, lentgh);
        }
    }
}
