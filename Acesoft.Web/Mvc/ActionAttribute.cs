using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Acesoft.Logger;

namespace Acesoft.Web.Mvc
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

            // log here
            var logger = LoggerContext.GetLogger(context.ActionDescriptor.DisplayName);
            logger.LogDebug($"Execute WebApi action \"{Name}\" with \"{context.ActionDescriptor.DisplayName}\"");
        }
    }
}
