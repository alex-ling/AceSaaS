using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Util
{
    public static class BinaryHelper
    {
        public static bool GetHexBit(string hex, int position)
        {
            return (NaryHelper.HexToInt(hex) & GetPower(position)) > 0;
        }

        public static int GetPower(int pos)
        {
            return (int)Math.Pow(2, pos);
        }
    }
}
