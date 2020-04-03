using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.Pay.Entity
{
    [Table("pay_order")]
    public class Pay_Order : EntityBase
    {
        public long Ref_Id { get; set; }
        public long User_Id { get; set; }
        public string Name { get; set; }
        public string Order_SN { get; set; }
        public decimal Order_Money { get; set; }
        public string Remark { get; set; }

        public string Pay_Id { get; set; }
        public PayType? Pay_Type { get; set; }
        public decimal? Pay_Money { get; set; }
        public string Pay_Time { get; set; }
        public OrderState State { get; set; }

        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
    }
}
