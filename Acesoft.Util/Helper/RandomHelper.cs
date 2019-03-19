using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Util
{
    public sealed class RandomHelper
    {
        public static string GetRandomNumber(int length)
        {
            var rnd = new Random(Environment.TickCount);
            var rv = string.Empty;
            for (var i = 0; i < length; i++)
            {
                rv += rnd.Next(1, 10).ToString();
            }
            return rv;
        }

        public static string GetRandomHex(int length)
        {
            Random random = new Random(Environment.TickCount);
            string text = string.Empty;
            for (int i = 0; i < length; i++)
            {
                text += "0123456789ABCDEF"[random.Next(0, 15)].ToString();
            }
            return text;
        }
    }
}
