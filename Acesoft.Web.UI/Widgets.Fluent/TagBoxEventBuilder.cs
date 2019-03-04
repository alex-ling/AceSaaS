using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TagBoxEventBuilder : ComboBoxEventBuilder
	{
		public TagBoxEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public TagBoxEventBuilder OnTagFormatter(string handler)
		{
			Handler(TagBox.OnTagFormatter.EventName, handler);
			return this;
		}

		public TagBoxEventBuilder OnTagStyle(string handler)
		{
			Handler(TagBox.OnTagStyle.EventName, handler);
			return this;
		}

		public TagBoxEventBuilder OnClickTag(string handler)
		{
			Handler(TagBox.OnClickTag.EventName, handler);
			return this;
		}

		public TagBoxEventBuilder OnBeforeRemoveTag(string handler)
		{
			Handler(TagBox.OnBeforeRemoveTag.EventName, handler);
			return this;
		}

		public TagBoxEventBuilder OnRemoveTag(string handler)
		{
			Handler(TagBox.OnRemoveTag.EventName, handler);
			return this;
		}
	}
}
