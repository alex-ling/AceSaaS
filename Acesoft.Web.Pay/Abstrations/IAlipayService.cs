using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Web.Pay.Models;
using Essensoft.AspNetCore.Payment.Alipay.Notify;
using Essensoft.AspNetCore.Payment.Alipay.Response;

namespace Acesoft.Web.Pay
{
    public interface IAlipayService
    {
        Task<AlipayTradePrecreateResponse> PreCreate(PayRequest request);
        Task<AlipayTradePayResponse> Pay(PayRequest request);
        Task<AlipayTradeAppPayResponse> AppPay(PayRequest request);
        Task<AlipayTradePagePayResponse> PagePay(PayRequest request);
        Task<AlipayTradeWapPayResponse> WapPay(PayRequest request);

        Task<AlipayTradeQueryResponse> Query(string order_sn);
        Task<int> Notify(long orderId);
    }
}
