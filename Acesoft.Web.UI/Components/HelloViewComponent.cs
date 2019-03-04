using Microsoft.AspNetCore.Mvc;

namespace Acesoft.Web.UI.Components
{
	public class HelloViewComponent : ComponentBase
	{
		public override IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
