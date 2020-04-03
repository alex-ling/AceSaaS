using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Pay.Models
{
    public class PayRequest
    {
        public long Id { get; set; }

        // 退款
        public decimal Refund_Money { get; set; }
        public string Refund_Desc { get; set; }

        // 条码付授权码
        public string AuthCode { get; set; }
        public string ProduceCode { get; set; }
        public string OpenId { get; set; }

        public string NotifyUrl { get; set; }
        public string ReturnUrl { get; set; }
    }
}
