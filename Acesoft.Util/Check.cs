using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Acesoft
{
    public sealed class Check
    {
        public static void Require(bool assertion, string msg, string negativeMsg = null)
        {
            if (!assertion) throw new AceException(msg);
            else if (negativeMsg.HasValue()) throw new AceException(negativeMsg);
        }

        public static void Assert(bool assertion, string msg, string negativeMsg = null)
        {
            if (assertion) throw new AceException(msg);
            else if (negativeMsg.HasValue()) throw new AceException(negativeMsg);
        }
    }
}
