using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.Pay.Entity
{
    [Table("pay_refund")]
    public class Pay_Refund : EntityBase
    {
        public long Order_Id { get; set; }
        public string Order_SN { get; set; }
        public decimal Order_Money { get; set; }
        public string Order_Desc { get; set; }

        public string Refund_Id { get; set; }
        public decimal? Refund_Money { get; set; }
        public string Refund_Time { get; set; }
        public RefundState State { get; set; }

        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}
