using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Acesoft.Web.Database;
using Microsoft.AspNetCore.Mvc;

namespace Acesoft.Web.Mvc.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TestController : Controller
    {
        private IDatabaseStore store;
        public TestController(IDatabaseStore store)
        {
            this.store = store;
        }

        public IActionResult Get()
        {
            this.store.CreateTables();
            return Ok("ok");
        }

        public IActionResult Delete()
        {
            this.store.DropTables();
            return Ok("ok");
        }
    }
}
