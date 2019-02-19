using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Acesoft.Web.Mvc.Controllers
{
    public class TestController : Controller
    {
        private readonly IApplicationContext ctx;

        public TestController(IApplicationContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var id = ctx.DbSession.ExecuteScalar("select 1");
            return Json(id);
        }
    }
}