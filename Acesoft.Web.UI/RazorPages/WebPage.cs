using Microsoft.AspNetCore.Authorization;

namespace Acesoft.Web.UI.RazorPages
{
    /// <summary>
    /// WebPage is for default authorization
    /// </summary>
	[Authorize]
	public abstract class WebPage : WebPageBase
	{
	}
}
