using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class FormEventBuilder : EventBuilder
	{
		public FormEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public FormEventBuilder OnChange(string handler)
		{
			Handler(Form.OnChange.EventName, handler);
			return this;
		}

		public FormEventBuilder OnProgress(string handler)
		{
			Handler(Form.OnProgress.EventName, handler);
			return this;
		}

		public FormEventBuilder OnSubmit(string handler)
		{
			Handler(Form.OnSubmit.EventName, handler);
			return this;
		}

		public FormEventBuilder OnSuccess(string handler)
		{
			Handler(Form.OnSuccess.EventName, handler);
			return this;
		}
	}
}
