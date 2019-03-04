using System.Collections.Generic;

namespace Acesoft.Web.UI.Html
{
	public interface IHtmlBuilder
	{
		IHtmlNode Build();

		IDictionary<string, object> BuildOptions();
	}
}
