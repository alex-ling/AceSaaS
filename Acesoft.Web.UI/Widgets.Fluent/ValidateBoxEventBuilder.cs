using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ValidateBoxEventBuilder : EventBuilder
	{
		public ValidateBoxEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public ValidateBoxEventBuilder OnBeforeValidate(string handler)
		{
			Handler(ValidateBox.OnBeforeValidate.EventName, handler);
			return this;
		}

		public ValidateBoxEventBuilder OnValidate(string handler)
		{
			Handler(ValidateBox.OnValidate.EventName, handler);
			return this;
		}
	}
}
