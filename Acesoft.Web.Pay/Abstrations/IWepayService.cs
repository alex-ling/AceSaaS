using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Web.Pay.Models;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.Response;

namespace Acesoft.Web.Pay
{
    public interface IWepayService
    {
        Task<WeChatPayMicroPayResponse> MicroPay(PayRequest request);
        Task<WeChatPayDictionary> PubPay(PayRequest request);
        Task<WeChatPayUnifiedOrderResponse> QrCodePay(PayRequest request);
        Task<WeChatPayDictionary> AppPay(PayRequest request);
        Task<WeChatPayUnifiedOrderResponse> H5Pay(PayRequest request);
        Task<WeChatPayDictionary> LiteAppPay(PayRequest request);

        Task<WeChatPayOrderQueryResponse> OrderQuery(string order_sn);
        Task<WeChatPayRiskGetPublicKeyResponse> GetPublicKey();
    }
}
