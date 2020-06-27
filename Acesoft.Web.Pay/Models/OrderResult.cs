using Acesoft.Web.Pay.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Pay.Models
{
    public class OrderResult
    {
        public Pay_Order Order { get; set; }

        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
