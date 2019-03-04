using Microsoft.AspNetCore.Mvc;

namespace Acesoft.Web.UI.Components
{
	public abstract class ComponentBase : ViewComponent, IViewComponent
	{
		public abstract IViewComponentResult Invoke();
	}
}
