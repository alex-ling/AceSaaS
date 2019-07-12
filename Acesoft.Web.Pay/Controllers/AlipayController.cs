using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Essensoft.AspNetCore.Payment.Alipay;
using Acesoft.Web;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Web.Pay.Models;
using Essensoft.AspNetCore.Payment.Alipay.Domain;
using Essensoft.AspNetCore.Payment.Alipay.Request;
using Essensoft.AspNetCore.Payment.Alipay.Notify;
using Acesoft.Web.Pay.Entity;

namespace Acesoft.Web.Pay.Controllers
{
    [ApiExplorerSettings(GroupName = "plat")]
    [Route("api/[controller]/[action]")]
    public class AlipayController : ApiControllerBase
    {
        private readonly IAlipayClient client;
        private readonly IAlipayNotifyClient notifyClient;
        private readonly IOrderService orderService;

        public AlipayController(IAlipayClient client, 
            IAlipayNotifyClient notifyClient,
            IOrderService orderService)
        {
            this.client = client;
            this.notifyClient = notifyClient;
            this.orderService = orderService;
        }

        [HttpPost, MultiAuthorize, Action("网站支付")]
        public async Task<IActionResult> PostPay(PayRequest request)
        {
            var order = orderService.GetByRef(request.Id);
            var model = new AlipayTradePagePayModel
            {
                Body = order.Remark,
                Subject = order.Name,
                TotalAmount = order.Order_Money.ToString("n"),
                OutTradeNo = order.Order_SN,
                ProductCode = "FAST_INSTANT_TRADE_PAY"
            };
            var req = new AlipayTradePagePayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(request.NotifyUrl);
            req.SetReturnUrl(request.ReturnUrl);

            var response = await client.PageExecuteAsync(req);
            return Content(response.Body, "text/html", Encoding.UTF8);
        }

        [HttpGet, MultiAuthorize, Action("网站支付通知")]
        public async Task<IActionResult> GetReturn(long orderId)
        {
            return Ok(await orderService.AlipayNotify(notifyClient, orderId));
        }

        #region pay
        [HttpPost, MultiAuthorize, Action("当面付(扫码支付)")]
        public async Task<IActionResult> PreCreate(AlipayTradePreCreateViewModel viewModel)
        {
            var model = new AlipayTradePrecreateModel
            {
                OutTradeNo = viewModel.OutTradeNo,
                Subject = viewModel.Subject,
                TotalAmount = viewModel.TotalAmount,
                Body = viewModel.Body
            };
            var req = new AlipayTradePrecreateRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(viewModel.NotifyUrl);

            var response = await client.ExecuteAsync(req);
            return Ok(response);
        }

        [HttpPost, MultiAuthorize, Action("当面付(二维码/条码/声波支付)")]
        public async Task<IActionResult> Pay(AlipayTradePayViewModel viewModel)
        {
            var model = new AlipayTradePayModel
            {
                OutTradeNo = viewModel.OutTradeNo,
                Subject = viewModel.Subject,
                Scene = viewModel.Scene,
                AuthCode = viewModel.AuthCode,
                TotalAmount = viewModel.TotalAmount,
                Body = viewModel.Body
            };
            var req = new AlipayTradePayRequest();
            req.SetBizModel(model);

            var response = await client.ExecuteAsync(req);
            return Ok(response);
        }

        [HttpPost, MultiAuthorize, Action("APP支付")]
        public async Task<IActionResult> AppPay(AlipayTradeAppPayViewModel viewModel)
        {
            var model = new AlipayTradeAppPayModel
            {
                OutTradeNo = viewModel.OutTradeNo,
                Subject = viewModel.Subject,
                ProductCode = viewModel.ProductCode,
                TotalAmount = viewModel.TotalAmount,
                Body = viewModel.Body
            };
            var req = new AlipayTradeAppPayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(viewModel.NotifyUrl);

            var response = await client.SdkExecuteAsync(req);
            //将response.Body给 ios/android端 由其去调起支付宝APP
            //(https://docs.open.alipay.com/204/105296/ https://docs.open.alipay.com/204/105295/)
            return Ok(response);
        }

        [HttpPost, MultiAuthorize, Action("电脑网站支付")]
        public async Task<IActionResult> PagePay(AlipayTradePagePayViewModel viewModel)
        {
            var model = new AlipayTradePagePayModel
            {
                Body = viewModel.Body,
                Subject = viewModel.Subject,
                TotalAmount = viewModel.TotalAmount,
                OutTradeNo = viewModel.OutTradeNo,
                ProductCode = viewModel.ProductCode
            };
            var req = new AlipayTradePagePayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(viewModel.NotifyUrl);
            req.SetReturnUrl(viewModel.ReturnUrl);

            var response = await client.PageExecuteAsync(req);
            return Content(response.Body, "text/html", Encoding.UTF8);
        }

        [HttpPost, MultiAuthorize, Action("手机网站支付")]
        public async Task<IActionResult> WapPay(AlipayTradeWapPayViewModel viewMode)
        {
            var model = new AlipayTradeWapPayModel
            {
                Body = viewMode.Body,
                Subject = viewMode.Subject,
                TotalAmount = viewMode.TotalAmount,
                OutTradeNo = viewMode.OutTradeNo,
                ProductCode = viewMode.ProductCode
            };
            var req = new AlipayTradeWapPayRequest();
            req.SetBizModel(model);
            req.SetNotifyUrl(viewMode.NotifyUrl);
            req.SetReturnUrl(viewMode.ReturnUrl);

            var response = await client.PageExecuteAsync(req);
            return Content(response.Body, "text/html", Encoding.UTF8);
        }

        [HttpPost, MultiAuthorize, Action("交易查询")]
        public async Task<IActionResult> Query(AlipayTradeQueryViewModel viewMode)
        {
            var model = new AlipayTradeQueryModel
            {
                OutTradeNo = viewMode.OutTradeNo,
                TradeNo = viewMode.TradeNo
            };

            var req = new AlipayTradeQueryRequest();
            req.SetBizModel(model);

            var response = await client.ExecuteAsync(req);
            return Ok(response);
        }

        [HttpPost, MultiAuthorize, Action("交易退款")]
        public async Task<IActionResult> Refund(AlipayTradeRefundViewModel viewMode)
        {
            var model = new AlipayTradeRefundModel
            {
                OutTradeNo = viewMode.OutTradeNo,
                TradeNo = viewMode.TradeNo,
                RefundAmount = viewMode.RefundAmount,
                OutRequestNo = viewMode.OutRequestNo,
                RefundReason = viewMode.RefundReason
            };

            var req = new AlipayTradeRefundRequest();
            req.SetBizModel(model);

            var response = await client.ExecuteAsync(req);
            return Ok(response);
        }

        [HttpPost, MultiAuthorize, Action("退款查询")]
        public async Task<IActionResult> RefundQuery(AlipayTradeRefundQueryViewModel viewMode)
        {
            var model = new AlipayTradeFastpayRefundQueryModel
            {
                OutTradeNo = viewMode.OutTradeNo,
                TradeNo = viewMode.TradeNo,
                OutRequestNo = viewMode.OutRequestNo
            };

            var req = new AlipayTradeFastpayRefundQueryRequest();
            req.SetBizModel(model);

            var response = await client.ExecuteAsync(req);
            ViewData["response"] = response.Body;
            return View();
        }

        [HttpPost, MultiAuthorize, Action("单笔转账到支付宝账户")]
        public async Task<IActionResult> Transfer(AlipayTransferViewModel viewMode)
        {
            var model = new AlipayFundTransToaccountTransferModel
            {
                OutBizNo = viewMode.OutBizNo,
                PayeeType = viewMode.PayeeType,
                PayeeAccount = viewMode.PayeeAccount,
                Amount = viewMode.Amount,
                Remark = viewMode.Remark
            };
            var req = new AlipayFundTransToaccountTransferRequest();
            req.SetBizModel(model);
            var response = await client.ExecuteAsync(req);
            ViewData["response"] = response.Body;
            return View();
        }

        [HttpPost, MultiAuthorize, Action("查询转账订单")]
        public async Task<IActionResult> TransQuery(AlipayTransQueryViewModel viewMode)
        {
            var model = new AlipayFundTransOrderQueryModel
            {
                OutBizNo = viewMode.OutBizNo,
                OrderId = viewMode.OrderId
            };

            var req = new AlipayFundTransOrderQueryRequest();
            req.SetBizModel(model);
            var response = await client.ExecuteAsync(req);
            ViewData["response"] = response.Body;
            return View();
        }

        [HttpPost, MultiAuthorize, Action("查询对账单下载地址")]
        public async Task<IActionResult> BillDownloadurlQuery(AlipayBillDownloadurlQueryViewModel viewModel)
        {
            var model = new AlipayDataDataserviceBillDownloadurlQueryModel
            {
                BillDate = viewModel.BillDate,
                BillType = viewModel.BillType
            };

            var req = new AlipayDataDataserviceBillDownloadurlQueryRequest();
            req.SetBizModel(model);
            var response = await client.ExecuteAsync(req);
            ViewData["response"] = response.Body;
            return View();
        }

        [HttpGet, MultiAuthorize, Action("电脑网站支付(回跳)")]
        public async Task<IActionResult> PagePayReturn()
        {
            var notify = await notifyClient.ExecuteAsync<AlipayTradePagePayReturn>(Request);
            return Ok(notify);
        }

        [HttpGet, MultiAuthorize, Action("手机网站支付(回跳)")]
        public async Task<IActionResult> WapPayReturn()
        {
            var notify = await notifyClient.ExecuteAsync<AlipayTradeWapPayReturn>(Request);
            return Ok(notify);
        }
        #endregion
    }
}
