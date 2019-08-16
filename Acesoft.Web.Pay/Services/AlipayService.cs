using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Web.Pay.Models;
using Essensoft.AspNetCore.Payment.Alipay;
using Essensoft.AspNetCore.Payment.Alipay.Domain;
using Essensoft.AspNetCore.Payment.Alipay.Notify;
using Essensoft.AspNetCore.Payment.Alipay.Request;
using Essensoft.AspNetCore.Payment.Alipay.Response;
using Microsoft.Extensions.Options;

namespace Acesoft.Web.Pay.Services
{
    public class AlipayService : IAlipayService
    {
        private readonly IAlipayClient client;
        private readonly IAlipayNotifyClient notifyClient;
        private readonly IOptions<AlipayOptions> options;
        private readonly IOrderService orderService;

        public AlipayService(IAlipayClient client,
            IAlipayNotifyClient notifyClient,
            IOptions<AlipayOptions> options,
            IOrderService orderService)
        {
            this.client = client;
            this.notifyClient = notifyClient;
            this.orderService = orderService;
            this.options = options;
        }

        // 当面付/扫码支付
        public async Task<AlipayTradePrecreateResponse> PreCreate(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var model = new AlipayTradePrecreateModel
            {
                OutTradeNo = order.Order_SN,
                Subject = order.Name,
                TotalAmount = order.Order_Money.ToString("n"),
                Body = order.Remark
            };

            var req = new AlipayTradePrecreateRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(request.NotifyUrl);

            return await client.ExecuteAsync(req, options.Value);
        }

        // 条码/声波支付
        public async Task<AlipayTradePayResponse> Pay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var model = new AlipayTradePayModel
            {
                OutTradeNo = order.Order_SN,
                Subject = order.Name,
                Scene = "bar_code", //wave_code
                AuthCode = request.AuthCode,
                TotalAmount = order.Order_Money.ToString("n"),
                Body = order.Remark,
                ProductCode = request.ProduceCode ?? "FACE_TO_FACE_PAYMENT"
            };

            var req = new AlipayTradePayRequest();
            req.SetBizModel(model);

            return await client.ExecuteAsync(req, options.Value);
        }

        // APP支付（集成SDK）
        public async Task<AlipayTradeAppPayResponse> AppPay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var model = new AlipayTradeAppPayModel
            {
                OutTradeNo = order.Order_SN,
                Subject = order.Name,
                TotalAmount = order.Order_Money.ToString("n"),
                Body = order.Remark,
                ProductCode = request.ProduceCode ?? "QUICK_MSECURITY_PAY"
            };

            var req = new AlipayTradeAppPayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(request.NotifyUrl);

            // 将response.ResponseBody ios/android端 由其去调起支付宝APP
            // https://docs.open.alipay.com/204/105296/ 
            // https://docs.open.alipay.com/204/105295/
            return await client.SdkExecuteAsync(req, options.Value);
        }

        // 电脑网站支付
        public async Task<AlipayTradePagePayResponse> PagePay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var model = new AlipayTradePagePayModel
            {
                OutTradeNo = order.Order_SN,
                Subject = order.Name,
                TotalAmount = order.Order_Money.ToString("n"),
                Body = order.Remark,
                ProductCode = "FAST_INSTANT_TRADE_PAY"
            };

            var req = new AlipayTradePagePayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(request.NotifyUrl);
            req.SetReturnUrl(request.ReturnUrl);

            return await client.PageExecuteAsync(req, options.Value);
        }

        // 手机网站支付
        public async Task<AlipayTradeWapPayResponse> WapPay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var model = new AlipayTradeWapPayModel
            {
                OutTradeNo = order.Order_SN,
                Subject = order.Name,
                TotalAmount = order.Order_Money.ToString("n"),
                Body = order.Remark,
                ProductCode = "QUICK_WAP_WAY"
            };

            var req = new AlipayTradeWapPayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(request.NotifyUrl);
            req.SetReturnUrl(request.ReturnUrl);

            return await client.PageExecuteAsync(req, options.Value);
        }

        // 通知
        public async Task<int> Notify(long orderId)
        {
            var res = await notifyClient.ExecuteAsync<AlipayTradePagePayReturn>(
                App.Context.Request, options.Value);
            if (res != null)
            {
                return orderService.Update(
                    orderId,
                    res.TotalAmount.ToObject<decimal>(),
                    res.Timestamp,
                    PayType.Alipay
                );
            }

            return 0;
        }

        // 查询
        public async Task<AlipayTradeQueryResponse> Query(string order_sn)
        {
            var model = new AlipayTradeQueryModel
            {
                OutTradeNo = order_sn
            };

            var req = new AlipayTradeQueryRequest();
            req.SetBizModel(model);

            return await client.ExecuteAsync(req, options.Value);
        }
    }
}