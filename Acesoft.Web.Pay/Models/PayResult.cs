using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Pay.Models
{
    public class PayResult
    {
        public IDictionary<string, string> Params { get; set; }

        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class PayWebResult : PayResult
    {
        public string WebUrl { get; set; }
    }

    public class PayQrResult : PayResult
    {
        public string CodeUrl { get; set; }
    }
}
