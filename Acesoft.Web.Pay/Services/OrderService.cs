using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acesoft.Data;
using Acesoft.Web.Pay.Entity;
using Acesoft.Web.Pay.Models;
using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.Alipay.Notify;

namespace Acesoft.Web.Pay.Services
{
    public class OrderService : Service<Pay_Order>, IOrderService
    {
        public int Paidup(long id, string payId, decimal payMoney, string payTime, PayType payType)
        {
            var sql = "update pay_order " +
                "set dupdate=@dupdate,pay_id=@payid,pay_money=@paymoney,pay_time=@paytime,pay_type=@paytype,status=2 " +
                "where id=@id and status=1";
            return Session.Execute(sql, new
            {
                id,
                dupdate = DateTime.Now,
                payId,
                payMoney,
                payTime,
                payType = (int)payType
            });
        }

        public Pay_Order Paidup(Pay_Order order, string payId, decimal payMoney, string payTime, PayType payType)
        {
            if (Paidup(order.Id, payId, payMoney, payTime, payType) > 0)
            {
                order.DUpdate = DateTime.Now;
                order.Pay_Id = payId;
                order.Pay_Money = payMoney;
                order.Pay_Time = payTime;
                order.Pay_Type = payType;
            }
            return order;
        }

        public Pay_Order Close(Pay_Order order, PayType payType)
        {
            order.State = OrderState.Closed;
            order.Pay_Type = payType;
            order.DUpdate = DateTime.Now;
            Session.Update(order);
            return order;
        }

        public Pay_Order Refund(Pay_Order order, PayType payType)
        {
            order.State = OrderState.Refunded;
            order.Pay_Type = payType;
            order.DUpdate = DateTime.Now;
            Session.Update(order);
            return order;
        }

        public Pay_Order Reverse(Pay_Order order, PayType payType)
        {
            order.State = OrderState.Reversed;
            order.Pay_Type = payType;
            order.DUpdate = DateTime.Now;
            Session.Update(order);
            return order;
        }

        public Pay_Refund GetRefund(Pay_Order order, decimal refundMoney, string refundDesc)
        {
            var sql = "select * from pay_refund where order_id=@orderid and status=1";
            var refund = Session.QueryFirst<Pay_Refund>(sql, new { orderId = order.Id });

            if (refund == null)
            {
                sql = "select isnull(sum(order_money),0) as refunded,count(*) as count " +
                    "from pay_refund where order_id=@orderid and status=2";
                var result = Session.QuerySingle<RefundedResult>(sql, new { orderId = order.Id });

                if (result.Refunded + refundMoney > order.Order_Money)
                {
                    throw new AceException($"退款金额大于支付金额！");
                }

                refund = new Pay_Refund();
                refund.InitializeId();
                refund.DCreate = DateTime.Now;
                refund.Order_Id = order.Id;
                refund.Order_SN = $"{order.Order_SN}_{result.Count}";
                refund.Order_Money = refundMoney;
                refund.Order_Desc = refundDesc;
                refund.State = RefundState.Refunding;
                Session.Insert(refund);
            }

            return refund;
        }

        public Pay_Refund Refunded(Pay_Refund refund, string refundId, decimal refundMoney, string refundTime)
        {
            refund.DUpdate = DateTime.Now;
            refund.Refund_Id = refundId;
            refund.Refund_Money = refundMoney;
            refund.Refund_Time = refundTime;
            Session.Update(refund);
            return refund;
        }
    }
}