using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TagBoxBuilder : ComboBoxBuilder<TagBox, TagBoxBuilder>
	{
		public TagBoxBuilder(TagBox component)
			: base(component)
		{
		}

		public TagBoxBuilder Events(Action<TagBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new TagBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
}
