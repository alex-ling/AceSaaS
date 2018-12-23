using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Acesoft.Web.Mvc.Areas.Area3.Controllers
{
    [Area("Area3")]
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}