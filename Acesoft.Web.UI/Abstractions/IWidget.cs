using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace Acesoft.Web.UI
{
	public interface IWidget : IHtml, IHtmlContent
	{
		string Widget { get; }
		string Id { get; }
        string Name { get; }
        HttpContext Context { get; }
        WidgetFactory Ace { get; }

        void Render();
		string ToHtml();
	}
}
