using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Data;
using Acesoft.Web.Pay.Entity;

namespace Acesoft.Web.Pay
{
    public interface IOrderService : IService<Pay_Order>
    {
        int Paidup(long id, string payId, decimal payMoney, string payTime, PayType payType);
        Pay_Order Paidup(Pay_Order order, string payId, decimal payMoney, string payTime, PayType payType);
        Pay_Order Close(Pay_Order order, PayType payType);
        Pay_Order Reverse(Pay_Order order, PayType payType);
        Pay_Order Refund(Pay_Order order, PayType payType);
        Pay_Refund GetRefund(Pay_Order order, decimal refundMoney, string refundDesc);
        Pay_Refund Refunded(Pay_Refund refund, string refundId, decimal refundMoney, string refundTime);
    }
}
