using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Web.Pay.Models;
using Essensoft.AspNetCore.Payment.Alipay;

namespace Acesoft.Web.Pay.Controllers
{
    [ApiExplorerSettings(GroupName = "plat")]
    [Route("api/[controller]/[action]")]
    public class AlipayController : ApiControllerBase
    {
        private readonly IAlipayService alipayService;

        public AlipayController(IAlipayService alipayService)
        {
            this.alipayService = alipayService;
        }

        [HttpPost, MultiAuthorize, Action("手机支付")]
        public async Task<IActionResult> AppPay(PayRequest req)
        {
            return Ok(await alipayService.AppPay(req));
        }

        [HttpPost, MultiAuthorize, Action("电脑网站支付")]
        public async Task<IActionResult> WebPay(PayRequest req)
        {
            var res = await alipayService.PagePay(req);
            return Content(res.ResponseBody, "text/html", Encoding.UTF8);
        }

        [HttpPost, MultiAuthorize, Action("手机网站支付")]
        public async Task<IActionResult> WapPay(PayRequest req)
        {
            var res = await alipayService.WapPay(req);
            return Content(res.ResponseBody, "text/html", Encoding.UTF8);
        }

        [HttpGet, Action("支付通知")]
        public async Task<IActionResult> Notify(long orderId)
        {
            if (await alipayService.Notify(orderId))
            {
                return AlipayNotifyResult.Success;
            }

            return NoContent();
        }
    }
}
