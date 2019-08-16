using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Web.Pay.Models;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.Request;
using Essensoft.AspNetCore.Payment.WeChatPay.Response;
using Microsoft.Extensions.Options;

namespace Acesoft.Web.Pay.Services
{
    public class WepayService : IWepayService
    {
        private readonly IWeChatPayClient client;
        private readonly IOptions<WeChatPayOptions> options;
        private readonly IOrderService orderService;

        public WepayService(IWeChatPayClient client,
            IOptions<WeChatPayOptions> options,
            IOrderService orderService)
        {
            this.client = client;
            this.options = options;
            this.orderService = orderService;
        }

        // 刷卡支付
        public async Task<WeChatPayMicroPayResponse> MicroPay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var req = new WeChatPayMicroPayRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Pay_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                AuthCode = request.AuthCode
            };
            return await client.ExecuteAsync(req, options.Value);
        }

        // 公众号支付
        public async Task<WeChatPayDictionary> PubPay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Pay_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.JSAPI,
                OpenId = request.OpenId
            };

            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                var jsapiReq = new WeChatPayJsApiSdkRequest
                {
                    Package = "prepay_id=" + res.PrepayId
                };

                // 将参数(parameter)给 公众号前端 让他在微信内H5调起支付
                // https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7&index=6
                return await client.ExecuteAsync(jsapiReq, options.Value);
            }

            throw new AceException($"公众号支付错误：{res.ErrCode}:{res.ErrCodeDes}");
        }

        // 扫码支付
        public async Task<WeChatPayUnifiedOrderResponse> QrCodePay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Pay_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.NATIVE
            };

            // res.CodeUrl 给前端生成二维码
            return await client.ExecuteAsync(req, options.Value);
        }

        // APP支付
        public async Task<WeChatPayDictionary> AppPay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Pay_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.APP
            };

            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                var appReq = new WeChatPayAppSdkRequest
                {
                    PrepayId = res.PrepayId
                };

                // 将参数(parameter)给 ios/android端 让他调起微信APP
                // https://pay.weixin.qq.com/wiki/doc/api/app/app.php?chapter=8_5
                return await client.ExecuteAsync(appReq, options.Value);
            }

            throw new AceException($"APP支付错误：{res.ErrCode}:{res.ErrCodeDes}");
        }

        // H5支付
        public async Task<WeChatPayUnifiedOrderResponse> H5Pay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Pay_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.MWEB
            };

            // mweb_url为拉起微信支付收银台的中间页面
            // 可通过访问该url来拉起微信客户端完成支付
            // res.mweb_url的有效期为5分钟
            return await client.ExecuteAsync(req, options.Value);
        }

        // 小程序支付
        public async Task<WeChatPayDictionary> LiteAppPay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Pay_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.JSAPI,
                OpenId = request.OpenId
            };

            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                var jsapiReq = new WeChatPayLiteAppSdkRequest
                {
                    Package = "prepay_id=" + res.PrepayId
                };

                // 将参数[res.parameter]给小程序前端让他调起支付API
                // https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=7_7&index=5
                return await client.ExecuteAsync(jsapiReq, options.Value);
            }

            throw new AceException($"APP支付错误：{res.ErrCode}:{res.ErrCodeDes}");
        }

        // 查询订单
        public async Task<WeChatPayOrderQueryResponse> OrderQuery(string order_sn)
        {
            var req = new WeChatPayOrderQueryRequest
            {
                OutTradeNo = order_sn
            };

            return await client.ExecuteAsync(req, options.Value);
        }

        // 获取公钥
        public async Task<WeChatPayRiskGetPublicKeyResponse> GetPublicKey()
        {
            var req = new WeChatPayRiskGetPublicKeyRequest();
            return await client.ExecuteAsync(req, options.Value);
        }
    }
}
