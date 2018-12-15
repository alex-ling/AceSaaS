using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Util
{
    public sealed class RandomHelper
    {
        public static string GetRandomNumberString(int length)
        {
            var rnd = new Random(Environment.TickCount);
            var rv = string.Empty;
            for (var i = 0; i < length; i++)
            {
                rv += rnd.Next(1, 10).ToString();
            }
            return rv;
        }
    }
}
