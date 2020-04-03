using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Web.Pay.Models;
using Essensoft.AspNetCore.Payment.WeChatPay;

namespace Acesoft.Web.Pay.Controllers
{
    [ApiExplorerSettings(GroupName = "plat")]
    [Route("api/[controller]/[action]")]
    public class WepayController : ApiControllerBase
    {
        private readonly IWepayService wepayService;

        public WepayController(IWepayService wepayService)
        {
            this.wepayService = wepayService;
        }

        [HttpGet, Action("支付通知")]
        public async Task<IActionResult> Notify(long orderId)
        {
            if (await wepayService.Notify(orderId))
            {
                return WeChatPayNotifyResult.Success;
            }

            return NoContent();
        }

        [HttpGet, Action("退款通知")]
        public async Task<IActionResult> RefundNotify(long refundId)
        {
            if (await wepayService.RefundNotify(refundId))
            {
                return WeChatPayNotifyResult.Success;
            }

            return NoContent();
        }
    }
}
