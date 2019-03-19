using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Util;

namespace Acesoft
{
    public static class ByteExtensions
    {
        public static string ToBase64(this byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static string ToUtf8(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static string ToHex(this byte[] data)
        {
            return EncodingHelper.BytesToHex(data);
        }

        public static string ToHex(this IEnumerable<byte> data)
        {
            return EncodingHelper.BytesToHex(data);
        }

        public static string ToStr(this byte[] data, string encoding = "")
        {
            if (!encoding.HasValue())
            {
                encoding = EncodingHelper.IsUtf8Text(data) ? "utf-8" : "gbk";
            }

            return Encoding.GetEncoding(encoding).GetString(data);
        }
    }
}
