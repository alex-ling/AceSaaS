using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI.Builder
{
	public interface IBuilder : IRender, IHtmlContent
	{
		string ToHtml();
	}
}
