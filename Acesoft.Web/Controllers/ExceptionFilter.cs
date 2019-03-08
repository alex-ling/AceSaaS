using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Acesoft.Logger;
using Microsoft.AspNetCore.Mvc;

namespace Acesoft.Web.Controllers
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger logger = LoggerContext.GetLogger<ExceptionFilter>();

        public override void OnException(ExceptionContext context)
        {
            var ex = context.Exception.GetException();
            logger.LogError(ex, ex.Message);

            if (context.HttpContext.Request.GetAppName() == "api")
            {
                context.Result = new ObjectResult(new ApiResult
                {
                    http_status = 500,
                    error_code = "server_error",
                    error_msg = ex.Message
                })
                {
                    StatusCode = 500
                };
            }

            base.OnException(context);
        }
    }
}
