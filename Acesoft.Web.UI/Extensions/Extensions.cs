using System;
using System.IO;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Net.Http.Headers;

namespace Acesoft.Web.UI
{
	public static class Extensions
	{
		public static string ToHtml(this IHtmlContent component)
		{
			if (component == null)
			{
				return "";
			}
			StringWriter stringWriter = new StringWriter();
			component.Render(stringWriter);
			return stringWriter.ToString().Replace("&quot;", "'").Replace("&#x27;", "'");
		}

		public static void Render(this IHtmlContent component, TextWriter writer)
		{
			component.WriteTo(writer, App.DefaultEncoder);
		}

		public static void Render(this HelperResult template, TextWriter writer)
		{
			template.WriteTo(writer, App.DefaultEncoder);
		}

        public static void EnableCacheing(this HttpResponse res, int seconds)
        {
            res.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = new TimeSpan?(TimeSpan.FromSeconds(seconds))
            };
            res.Headers["Vary"] = new string[1]
            {
                "Accept-Encoding"
            };
        }
    }
}
