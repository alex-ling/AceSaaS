using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Acesoft.Web.UI.Components
{
	public abstract class AsyncComponentBase : ViewComponent, IViewComponent
	{
		public abstract Task<IViewComponentResult> InvokeAsync();
	}
}
