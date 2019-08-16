using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Web.Pay.Models;

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
    }
}
