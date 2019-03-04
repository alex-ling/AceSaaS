using System.Collections.Generic;

namespace Acesoft.Web.UI
{
	public interface IHtml
	{
		IDictionary<string, object> Attributes { get; }
    }
}
