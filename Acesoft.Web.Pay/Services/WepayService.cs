using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Web.Pay.Entity;
using Acesoft.Web.Pay.Models;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.Notify;
using Essensoft.AspNetCore.Payment.WeChatPay.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acesoft.Web.Pay.Services
{
    public class WepayService : IWepayService
    {
        private readonly ILogger<WepayService> logger;
        private readonly IWeChatPayClient client;
        private readonly IWeChatPayNotifyClient notifyClient;
        private readonly IOptions<WeChatPayOptions> options;

        public IOrderService OrderService { get; }

        public WepayService(ILogger<WepayService> logger,
            IWeChatPayClient client,
            IWeChatPayNotifyClient notifyClient,
            IOptions<WeChatPayOptions> options,
            IOrderService orderService)
        {
            this.logger = logger;
            this.client = client;
            this.notifyClient = notifyClient;
            this.options = options;
            this.OrderService = orderService;
        }

        #region card
        // 刷卡支付
        public async Task<OrderResult> MicroPay(PayRequest request)
        {
            var order = OrderService.Get(request.Id);
            var req = new WeChatPayMicroPayRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Order_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                AuthCode = request.AuthCode
            };

            logger.LogDebug($"Wepay MicroPay begin with: {request.Id}");
            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                logger.LogDebug($"Wepay MicroPay end with: {request.Id} SUCCESS");
                return new OrderResult
                {
                    Success = true,
                    Order = OrderService.Paidup(
                        order,
                        res.TransactionId,
                        (decimal)(res.TotalFee / 100.0),
                        res.TimeEnd,
                        PayType.Wechat
                    )
                };
            }

            logger.LogDebug($"Wepay MicroPay end with: {request.Id} Fail: {res.ErrCode}:{res.ErrCodeDes}");
            return new OrderResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }
        #endregion

        #region pub
        // 公众号支付
        public async Task<PayResult> PubPay(PayRequest request)
        {
            var order = OrderService.Get(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Order_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.JSAPI,
                OpenId = request.OpenId
            };

            logger.LogDebug($"Wepay PubPay begin with: {request.Id}");
            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                var jsapiReq = new WeChatPayJsApiSdkRequest
                {
                    Package = "prepay_id=" + res.PrepayId
                };

                logger.LogDebug($"Wepay PubPay begin with: {request.Id} SUCCESS");

                // 将参数(parameter)给 公众号前端 让他在微信内H5调起支付
                // https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7&index=6
                return new PayResult
                {
                    Success = true,
                    Params = await client.ExecuteAsync(jsapiReq, options.Value)
                };
            }

            logger.LogDebug($"Wepay PubPay end with: {request.Id} Fail: {res.ErrCode}:{res.ErrCodeDes}");
            return new PayResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }
        #endregion

        #region qrcode
        // 扫码支付
        public async Task<PayQrResult> QrCodePay(PayRequest request)
        {
            var order = OrderService.Get(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Order_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.NATIVE
            };

            // res.CodeUrl 给前端生成二维码
            logger.LogDebug($"Wepay QrCodePay begin with: {request.Id}");
            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                logger.LogDebug($"Wepay QrCodePay end with: {request.Id} SUCCESS");
                return new PayQrResult
                {
                    Success = true,
                    CodeUrl = res.CodeUrl,
                    Params = res.ResponseParameters
                };
            }

            logger.LogDebug($"Wepay QrCodePay end with: {request.Id} Fail: {res.ErrCode}:{res.ErrCodeDes}");
            return new PayQrResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }
        #endregion

        #region app
        // APP支付
        public async Task<PayResult> AppPay(PayRequest request)
        {
            var order = OrderService.Get(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Order_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.APP
            };

            logger.LogDebug($"Wepay AppPay begin with: {request.Id}");
            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                var appReq = new WeChatPayAppSdkRequest
                {
                    PrepayId = res.PrepayId
                };

                logger.LogDebug($"Wepay AppPay end with: {request.Id} SUCCESS");

                // 将参数(parameter)给 ios/android端 让他调起微信APP
                // https://pay.weixin.qq.com/wiki/doc/api/app/app.php?chapter=8_5
                return new PayResult
                {
                    Success = true,
                    Params = await client.ExecuteAsync(appReq, options.Value)
                };
            }

            logger.LogDebug($"Wepay AppPay end with: {request.Id} Fail: {res.ErrCode}:{res.ErrCodeDes}");
            return new PayResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }
        #endregion

        #region web
        // H5支付
        public async Task<PayWebResult> H5Pay(PayRequest request)
        {
            var order = OrderService.Get(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Order_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.MWEB
            };

            // mweb_url为拉起微信支付收银台的中间页面
            // 可通过访问该url来拉起微信客户端完成支付
            // res.mweb_url的有效期为5分钟
            logger.LogDebug($"Wepay H5Pay begin with: {request.Id}");
            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                logger.LogDebug($"Wepay H5Pay end with: {request.Id} SUCCESS");
                return new PayWebResult
                {
                    Success = true,
                    WebUrl = res.MwebUrl,
                    Params = res.ResponseParameters
                };
            }

            logger.LogDebug($"Wepay H5Pay end with: {request.Id} Fail: {res.ErrCode}:{res.ErrCodeDes}");
            return new PayWebResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }
        #endregion

        #region liteapp
        // 小程序支付
        public async Task<PayResult> LiteAppPay(PayRequest request)
        {
            var order = OrderService.Get(request.Id);
            var req = new WeChatPayUnifiedOrderRequest
            {
                OutTradeNo = order.Order_SN,
                Body = order.Name,
                TotalFee = (int)(order.Order_Money * 100),
                SpBillCreateIp = App.Context.GetClientIp(),
                NotifyUrl = request.NotifyUrl,
                TradeType = WepayTypes.JSAPI,
                OpenId = request.OpenId
            };

            logger.LogDebug($"Wepay LiteAppPay begin with: {request.Id}");
            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                var jsapiReq = new WeChatPayLiteAppSdkRequest
                {
                    Package = "prepay_id=" + res.PrepayId
                };

                logger.LogDebug($"Wepay LiteAppPay end with: {request.Id} SUCCESS");

                // 将参数[res.parameter]给小程序前端让他调起支付API
                // https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=7_7&index=5
                return new PayResult
                {
                    Success = true,
                    Params = await client.ExecuteAsync(jsapiReq, options.Value)
                };
            }

            logger.LogDebug($"Wepay LiteAppPay end with: {request.Id} Fail: {res.ErrCode}:{res.ErrCodeDes}");
            return new PayResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }
        #endregion

        #region notify
        // 付款通知
        public async Task<bool> Notify(long orderId)
        {
            try
            {
                var res = await notifyClient.ExecuteAsync<WeChatPayUnifiedOrderNotify>(
                    App.Context.Request, options.Value);
                if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
                {
                    logger.LogDebug($"Wepay Notify with: {orderId} SUCCESS");
                    OrderService.Paidup(
                        orderId,
                        res.TransactionId,
                        (decimal)(res.TotalFee / 100.0),
                        res.TimeEnd,
                        PayType.Wechat
                    );
                    return true;
                }

                logger.LogDebug($"Wepay Notify with: {orderId} FAIL: {res.ErrCode}:{res.ErrCodeDes}");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Wepay Notify with: {orderId} FAIL: {ex.GetMessage()}");
                return false;
            }
        }
        #endregion

        #region close
        public async Task<OrderResult> Close(Pay_Order order)
        {
            if (order.State != OrderState.Pending)
            {
                // 只有待支付时可关闭订单
                return new OrderResult
                {
                    Success = false,
                    Error = $"只有未支付的订单可关闭"
                };
            }

            var request = new WeChatPayCloseOrderRequest
            {
                OutTradeNo = order.Order_SN
            };

            logger.LogDebug($"Wepay Close begin with: {order.Id}");
            var res = await client.ExecuteAsync(request, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                logger.LogDebug($"Wepay Close end with: {order.Id} SUCCESS");
                return new OrderResult
                {
                    Success = true,
                    Order = OrderService.Close(order, PayType.Wechat)
                };
            }

            logger.LogDebug($"Wepay Close end with: {order.Id} FAIL: {res.ErrCode}:{res.ErrCodeDes}");
            return new OrderResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }
        #endregion

        #region reverse
        public async Task<OrderResult> Reverse(Pay_Order order)
        {
            if (order.State != OrderState.Paidup)
            {
                // 只有已支付时可撤销订单
                return new OrderResult
                {
                    Success = false,
                    Error = $"只有已支付的订单可撤销"
                };
            }

            var request = new WeChatPayReverseRequest
            {
                TransactionId = order.Pay_Id
            };

            logger.LogDebug($"Wepay Reverse begin with: {order.Id}");
            var res = await client.ExecuteAsync(request, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                logger.LogDebug($"Wepay Reverse end with: {order.Id} SUCCESS");
                return new OrderResult
                {
                    Success = true,
                    Order = OrderService.Reverse(order, PayType.Wechat)
                };
            }

            logger.LogDebug($"Wepay Reverse end with: {order.Id} FAIL: {res.ErrCode}:{res.ErrCodeDes}");
            return new OrderResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }
        #endregion

        #region refund
        public async Task<RefundResult> Refund(PayRequest request)
        {
            var order = OrderService.Get(request.Id);
            var refund = OrderService.GetRefund(order, request.Refund_Money, request.Refund_Desc);
            var req = new WeChatPayRefundRequest
            {
                OutRefundNo = refund.Order_SN,
                TransactionId = order.Pay_Id,
                OutTradeNo = order.Order_SN,
                TotalFee = (int)(order.Pay_Money * 100),
                RefundFee = (int)(refund.Order_Money * 100),
                RefundDesc = refund.Order_Desc,
                NotifyUrl = request.NotifyUrl
            };

            logger.LogDebug($"Wepay Refund begin with: {request.Id}");
            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                logger.LogDebug($"Wepay Refund end with: {request.Id} SUCCESS");

                order = OrderService.Refund(order, PayType.Wechat);
                return new RefundResult
                {
                    Success = true,
                    Order = order,
                    Refund = OrderService.Refunded(
                        refund,
                        res.RefundId,
                        (decimal)(res.RefundFee / 100.0),
                        DateTime.Now.ToStr("yyyyMMddHHmmss")
                    )
                };
            }

            logger.LogDebug($"Wepay Refund end with: {request.Id} FAIL: {res.ErrCode}:{res.ErrCodeDes}");
            return new RefundResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }

        public async Task<bool> RefundNotify(long refundId)
        {
            try
            {
                var res = await notifyClient.ExecuteAsync<WeChatPayRefundNotify>(
                    App.Context.Request, options.Value);
                if (res.ReturnCode == "SUCCESS" && res.RefundStatus == "SUCCESS")
                {
                    logger.LogDebug($"Wepay RefundNotify with: {refundId} SUCCESS");
                    // 此处异步队列通知，退款同步时已更新结果
                    return true;
                }

                logger.LogDebug($"Wepay RefundNotify with: {refundId} FAIL: {res.ReturnMsg}");
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region query
        // 查询订单
        public async Task<OrderResult> Query(Pay_Order order)
        {
            var req = new WeChatPayOrderQueryRequest
            {
                OutTradeNo = order.Order_SN
            };

            logger.LogDebug($"Wepay Query begin with: {order.Id}");
            var res = await client.ExecuteAsync(req, options.Value);
            if (res.ReturnCode == "SUCCESS" && res.ResultCode == "SUCCESS")
            {
                logger.LogDebug($"Wepay Query end with: {order.Id} {res.TradeState}");
                if (order.State == OrderState.Pending)
                {
                    if (res.TradeState == "SUCCESS")
                    {
                        logger.LogDebug($"Wepay Query Paidup with: {order.Id}");
                        order = OrderService.Paidup(
                            order,
                            res.TransactionId,
                            (decimal)(res.TotalFee / 100.0),
                            res.TimeEnd,
                            PayType.Wechat
                        );
                    }
                    else if (res.TradeState == "CLOSED")
                    {
                        order = OrderService.Close(order, PayType.Wechat);
                    }
                    else if (res.TradeState == "REFUND")
                    {
                        order = OrderService.Refund(order, PayType.Wechat);
                    }
                    else if (res.TradeState == "REVOKED")
                    {
                        order = OrderService.Reverse(order, PayType.Wechat);
                    }
                }

                return new OrderResult
                {
                    Success = true,
                    Order = order
                };
            }

            logger.LogDebug($"Wepay Query end with: {order.Id} FAIL: {res.ErrCode}:{res.ErrCodeDes}");
            return new OrderResult
            {
                Success = false,
                Error = $"{res.ErrCode}:{res.ErrCodeDes}"
            };
        }
        #endregion

        #region publickey
        // 获取公钥
        public async Task<PayPublicKey> GetPublicKey()
        {
            var req = new WeChatPayRiskGetPublicKeyRequest();
            var res = await client.ExecuteAsync(req, options.Value);
            return new PayPublicKey
            {
                MchId = res.MchId,
                PubKey = res.PubKey
            };
        }
        #endregion
    }
}
