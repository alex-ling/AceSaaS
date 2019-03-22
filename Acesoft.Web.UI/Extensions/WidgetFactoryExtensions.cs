using System.Collections.Generic;
using System.Text;
using Acesoft.Util.Helper;
using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI
{
	public static class WidgetFactoryExtensions
	{
        public static string GetAppUrl(this WidgetFactory ace, string url, params string[] appendQueries)
        {
            url = $"/{ace.AppName}/{url}";
            foreach (var query in appendQueries)
            {
                url = UrlHelper.Append(url, query, App.GetQuery(query, ""));
            }
            return url;
        }

		public static IHtmlContent RenderInitScripts(this WidgetFactory ace, bool renderTags = true)
		{
			var sb = new StringBuilder();
			if (renderTags)
			{
				sb.Append("<script>");
			}

            var settings = App.AppConfig.Settings;
			var loginUrl = settings.GetValue("auth.loginurl", "/plat/account/login");
			var logoutUrl = settings.GetValue("auth.logouturl", "/plat/account/logout");
            var webRoot = App.GetWebRoot();

			sb.Append($"AX.init({{root:'{webRoot}',path:'{ace.Path}',app:'{ace.AppName}'");
			if (ace.RenderAuthOptions)
			{
				sb.Append($",loginUrl:'{loginUrl}',logoutUrl:'{logoutUrl}'");
			}
			sb.Append("});");

            // append script init.
			sb.Append(App.Context.GetInitScripts());

			if (renderTags)
			{
				sb.Append("</script>");
			}
			return new HtmlString(sb.ToString());
		}
	}
}
