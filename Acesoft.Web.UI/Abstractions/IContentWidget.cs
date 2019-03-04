using System.Collections.Generic;

using Acesoft.Web.UI.Html;
using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI
{
	public interface IContentWidget
	{
		object Data { get; set; }
		IHtmlTemplate Template { get; }
		IList<IHtmlContent> Controls { get; }
	}
}
