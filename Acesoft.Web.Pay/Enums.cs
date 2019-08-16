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

    public enum OrderStatus : int
    {
        Pending = 1,
        Paidup = 2
    }
}
