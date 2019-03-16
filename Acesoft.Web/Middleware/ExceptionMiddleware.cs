using System;
using System.Net;
using System.Threading.Tasks;

using Acesoft.Web.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Acesoft.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        //https://github.com/aspnet/Diagnostics/blob/master/src/
        public async Task Invoke(HttpContext context)
        {
            var isApiRequest = context.Request.GetAppName().ToLower() == "api";
            var error = "";

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                if (isApiRequest)
                {
                    // While api request, wrapper exception.
                    context.Response.StatusCode = 500;
                    error = ex.GetMessage();
                }
                else
                {
                    // While not api request, throw exception for UseExceptionHandler.
                    throw;
                }
            }
            finally
            {
                if (isApiRequest)
                {
                    var stausCode = context.Response.StatusCode;

                    if (!context.Response.HasStarted
                        && stausCode >= 400 && stausCode < 600
                        && !context.Response.ContentLength.HasValue
                        && string.IsNullOrEmpty(context.Response.ContentType))
                    {
                        switch (context.Response.StatusCode)
                        {
                            case 400:
                                error = "请求不合法";
                                break;
                            case 401:
                                error = "未授权";
                                break;
                            case 404:
                                error = "未找到服务";
                                break;
                            case 502:
                                error = "请求错误";
                                break;
                            default:
                                if (!error.HasValue())
                                {
                                    error = "未知错误";
                                }
                                break;
                        }

                        if (error.HasValue())
                        {
                            await HandleExceptionAsync(context, error);
                        }
                    }
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string error)
        {
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ApiResult
            {
                status = context.Response.StatusCode,
                error = error
            }));
        }
    }
}
