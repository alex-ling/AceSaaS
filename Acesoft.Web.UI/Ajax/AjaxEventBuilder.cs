using System.Collections.Generic;

namespace Acesoft.Web.UI.Ajax
{
	public class AjaxEventBuilder : EventBuilder
	{
		public AjaxEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public AjaxEventBuilder DataFilter(string handler)
		{
			Handler(AjaxObject.DataFilter.EventName, handler);
			return this;
		}

		public AjaxEventBuilder OnBeforeSend(string handler)
		{
			Handler(AjaxObject.OnBeforeSend.EventName, handler);
			return this;
		}

		public AjaxEventBuilder OnComplete(string handler)
		{
			Handler(AjaxObject.OnComplete.EventName, handler);
			return this;
		}

		public AjaxEventBuilder OnError(string handler)
		{
			Handler(AjaxObject.OnError.EventName, handler);
			return this;
		}

		public AjaxEventBuilder OnSuccess(string handler)
		{
			Handler(AjaxObject.OnSuccess.EventName, handler);
			return this;
		}
	}
}
