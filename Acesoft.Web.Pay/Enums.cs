using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Pay
{
    public enum PayType : int
    {
        Cash = 1,
        Alipay = 2,
        Wechat = 3,
        Unionpay = 4,
        Wallet = 5,
        JDPay = 6
    }

    public enum OrderState : int
    {
        Pending = 1,
        Paidup = 2,
        Closed = 3,
        Reversed = 4,
        Refunded = 5
    }

    public enum RefundState : int
    {
        Refunding = 1,
        Refunded = 2
    }
}
