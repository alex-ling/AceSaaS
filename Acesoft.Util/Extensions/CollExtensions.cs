using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acesoft.Util
{
    public static class CollExtensions
    {
        public static void Each<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }

        public static void Each<T>(this IEnumerable list, Action<T> action)
        {
            foreach (var item in list)
            {
                action((T)item);
            }
        }

        public static string Join(this IEnumerable list, string separator = ",")
        {
            var sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendFormat("{0}{1}", separator, item);
            }
            if (sb.Length > 0)
            {
                sb.Remove(0, separator.Length);
            }
            return sb.ToString();
        }

        public static string Join(this IEnumerable<string> list, string separator = ",")
        {
            return string.Join(separator, list);
        }

        public static string Join<T>(this IEnumerable<T> list, string separator = ",")
        {
            return string.Join(separator, list);
        }

        public static IList<T> CloneRange<T>(this IList<T> list, int offset, int length)
        {
            Check.Require(offset >= 0, "给定参数应满足[offset>=0]");
            Check.Require(offset + length <= list.Count, "给定参数应满足[offset+length<=count]");

            var result = new List<T>(length);
            for (var i = 0; i < length; i++)
            {
                result.Add(list[offset + i]);
            }
            return result;
        }

        public static IList<T> CloneRange<T>(this IEnumerable<T> list, int offset, int length)
        {
            return list.ToList().CloneRange(offset, length);
        }
    }
}
