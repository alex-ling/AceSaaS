using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft
{
    public static class ExceptionExtensions
    {
        public static string GetMessage(this Exception ex)
        {
            return ex.GetException().Message;
        }

        public static Exception GetException(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
    }
}
