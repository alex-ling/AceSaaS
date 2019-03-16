using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Acesoft.Logger;

namespace Acesoft.Web.Mvc
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var ex = context.Exception.GetException();
            var logger = LoggerContext.GetLogger(context.ActionDescriptor.DisplayName);
            logger.LogError(ex, "Execute with exception!!!");

            base.OnException(context);
        }
    }
}
