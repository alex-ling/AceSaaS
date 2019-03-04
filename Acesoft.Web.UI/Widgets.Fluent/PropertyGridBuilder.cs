using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class PropertyGridBuilder : DataGridBuilder<PropertyGrid, PropertyGridBuilder>
	{
		public PropertyGridBuilder(PropertyGrid component)
			: base(component)
		{
		}

		public virtual PropertyGridBuilder ShowGroup(bool showGroup = true)
		{
			base.Component.ShowGroup = showGroup;
			return this;
		}

		public virtual PropertyGridBuilder GroupField(string groupField)
		{
			base.Component.GroupField = groupField;
			return this;
		}

		public PropertyGridBuilder Events(Action<PropertyGridEventBuilder> clientEventsAction)
		{
			clientEventsAction(new PropertyGridEventBuilder(base.Component.Events));
			return this;
		}
	}
}
