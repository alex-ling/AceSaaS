using System;

using Acesoft.Rbac;

namespace Acesoft.Web.UI.RazorPages
{
    /// <summary>
    /// AppPage is for cookie and bearer authorization
    /// </summary>
	[MultiAuthorize]
	public abstract class AuthPage : WebPageBase
	{
	}
}
