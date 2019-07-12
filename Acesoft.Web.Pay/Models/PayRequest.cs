using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Pay.Models
{
    public class PayRequest
    {
        public long Id { get; set; }
        public string NotifyUrl { get; set; }
        public string ReturnUrl { get; set; }
    }
}
