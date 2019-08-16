using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Acesoft.Web.Models;

namespace Acesoft.Web.Mvc
{
    public class ApiResultFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result as OkObjectResult;
            if (result != null)
            {
                var value = result.Value as ApiResult;
                if (value != null)
                {
                    // change to null wrapper
                    result.Value = value.value;
                }
                else
                { 
                    // change to ApiResult wrapper
                    result.Value = new ApiResult
                    {
                        status = result.StatusCode ?? 200,
                        value = result.Value
                    };
                }
            }

            base.OnResultExecuting(context);
        }
    }
}
