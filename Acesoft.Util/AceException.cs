using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft
{
    public class AceException : Exception
    {
        public AceException() : base()
        { }

        public AceException(string message) : base(message)
        { }

        public AceException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
