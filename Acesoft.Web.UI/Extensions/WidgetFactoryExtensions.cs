using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI
{
	public static class WidgetFactoryExtensions
	{
		public static IHtmlContent RenderInitScripts(this WidgetFactory ace, bool renderTags = true)
		{
			var sb = new StringBuilder();
			if (renderTags)
			{
				sb.Append("<script>");
			}

            /*IDictionary<string, string> settings = ConfigFactory.GetConfig<AppConfig>().Settings;
			string value = settings.GetValue("auth.loginurl", "/plat/account/login");
			string value2 = settings.GetValue("auth.logouturl", "/plat/account/logout");
			stringBuilder.Append("AX.init({root:'" + WebHelper.WebRoot(false) + "',path:'" + ace.Path + "',app:'" + ace.AppName + "'");
			if (ace.RenderAuthOptions)
			{
				stringBuilder.Append(",loginUrl:'" + value + "',logoutUrl:'" + value2 + "'");
			}
			stringBuilder.Append("});");*/

            var initScripts = App.Context.GetInitScripts();
			if (initScripts.HasValue())
			{
				sb.Append(initScripts);
			}
			if (renderTags)
			{
				sb.Append("</script>");
			}
			return new HtmlString(sb.ToString());
		}
	}
}
