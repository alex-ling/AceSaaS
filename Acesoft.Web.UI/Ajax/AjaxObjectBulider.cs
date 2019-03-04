using System;

namespace Acesoft.Web.UI.Ajax
{
	public class AjaxObjectBulider : AjaxObjectBuilder<AjaxObject, AjaxObjectBulider>
	{
		public AjaxObjectBulider(AjaxObject ajaxObject)
			: base(ajaxObject)
		{
		}

		public AjaxObjectBulider Events(Action<AjaxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new AjaxEventBuilder(base.JsonObject.Events));
			return this;
		}
	}
}
