using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Acesoft.Web.Controllers
{
    public class ActionAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }

        public ActionAttribute(string name)
        {
            this.Name = name;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}
