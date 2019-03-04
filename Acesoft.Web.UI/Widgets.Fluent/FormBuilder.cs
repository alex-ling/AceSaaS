using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class FormBuilder : FormBuilder<Form, FormBuilder>
	{
		public FormBuilder(Form component)
			: base(component)
		{
		}
	}
	public class FormBuilder<Widget, Builder> : ContentBuilder<Widget, Builder> where Widget : Form where Builder : ContentBuilder<Widget, Builder>
	{
		public FormBuilder(Widget component)
			: base(component)
		{
		}

		public Builder Novalidate(bool novalidate = true)
		{
			base.Component.Novalidate = novalidate;
			return this as Builder;
		}

		public Builder Iframe(bool iframe = true)
		{
			base.Component.Iframe = iframe;
			return this as Builder;
		}

		public Builder Ajax(bool ajax = true)
		{
			base.Component.Ajax = ajax;
			return this as Builder;
		}

		public Builder Dirty(bool dirty = true)
		{
			base.Component.Dirty = dirty;
			return this as Builder;
		}

		public Builder Ajax(Action<DataSourceBuilder> ajaxAction)
		{
			ajaxAction(new DataSourceBuilder(base.Component.DataSource).Controller("crud"));
			return this as Builder;
		}

		public Builder Events(Action<FormEventBuilder> clientEventsAction)
		{
			clientEventsAction(new FormEventBuilder(base.Component.Events));
			return this as Builder;
		}
	}
}
