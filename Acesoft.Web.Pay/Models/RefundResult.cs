using Acesoft.Web.Pay.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Pay.Models
{
    public class RefundResult : OrderResult
    {
        public Pay_Refund Refund { get; set; }
    }
}
