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
    public class UnionpayController : ApiControllerBase
    {
        private readonly IUnionpayService unionpayService;

        public UnionpayController(IUnionpayService unionpayService)
        {
            this.unionpayService = unionpayService;
        }
    }
}
