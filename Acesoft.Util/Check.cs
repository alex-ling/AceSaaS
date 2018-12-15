using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Acesoft
{
    public sealed class Check
    {
        public static void Require(bool assertion, string message)
        {
            if (!assertion) throw new AceException(message);
        }

        public static void Require(bool assertion, string message, Exception inner)
        {
            if (!assertion) throw new AceException(message, inner);
        }
        
        public static void Require(bool assertion, string message, params string[] paramNames)
        {
            if (!assertion) throw new AceException(string.Format(message, paramNames));
        }
    }
}
