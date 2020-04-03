using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;
using Acesoft.Web.Pay.Entity;
using Acesoft.Web.Pay.Models;

namespace Acesoft.Web.Pay
{
    public interface IWepayService
    {
        IOrderService OrderService { get; }

        Task<OrderResult> MicroPay(PayRequest request);
        Task<PayResult> PubPay(PayRequest request);
        Task<PayQrResult> QrCodePay(PayRequest request);
        Task<PayResult> AppPay(PayRequest request);
        Task<PayWebResult> H5Pay(PayRequest request);
        Task<PayResult> LiteAppPay(PayRequest request);

        Task<bool> Notify(long orderId);
        Task<OrderResult> Close(Pay_Order order);
        Task<OrderResult> Reverse(Pay_Order order);
        Task<RefundResult> Refund(PayRequest request);
        Task<bool> RefundNotify(long orderId);
        Task<OrderResult> Query(Pay_Order order);
        Task<PayPublicKey> GetPublicKey();
    }
}
