using System;
using System.Collections.Generic;
using System.Text;

using RazorEngine;
using RazorEngine.Templating;
using Acesoft.Util;
using System.Security;
using RazorEngine.Configuration;

namespace Acesoft.Web.Razor
{
    public static class RazorHelper
    {
        [SecuritySafeCritical]
        public static string Generate<T>(string temp, T model)
        {
            //var config = new TemplateServiceConfiguration();
            //config.ReferenceResolver = new ReferenceResolver();
            //var service = RazorEngineService.Create(config);

            return RazorEngineServiceExtensions.RunCompile(
                Engine.Razor, 
                temp, 
                App.IdWorker.NextStringId(),
                typeof(T), 
                model, 
                null
            )
            .Replace("&quot;", "\"")
            .Replace("<c>", "")
            .Replace("</c>", "");
        }
    }
}
