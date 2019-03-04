using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class DataListBuilder : DataGridBuilder<DataList, DataListBuilder>
	{
		public DataListBuilder(DataList component)
			: base(component)
		{
		}

		public virtual DataListBuilder Lines(bool lines = true)
		{
			base.Component.Lines = lines;
			return this;
		}

		public virtual DataListBuilder ValueField(string valueField)
		{
			base.Component.ValueField = valueField;
			return this;
		}

		public virtual DataListBuilder TextField(string textField)
		{
			base.Component.TextField = textField;
			return this;
		}

		public virtual DataListBuilder GroupField(string groupField)
		{
			base.Component.GroupField = groupField;
			return this;
		}

		public DataListBuilder Events(Action<DataListEventBuilder> clientEventsAction)
		{
			clientEventsAction(new DataListEventBuilder(base.Component.Events));
			return this;
		}
	}
}
