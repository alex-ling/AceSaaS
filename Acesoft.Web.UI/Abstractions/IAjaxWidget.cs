using Acesoft.Web.UI.Ajax;
using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI
{
	public interface IAjaxWidget : IWidget
	{
		AjaxObject Ajax { get; }
    }
}
