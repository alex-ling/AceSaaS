using Acesoft.Web.UI.Ajax;
using System;
using System.Text;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ComboTreeBuilder : ComboBuilder<ComboTree, ComboTreeBuilder>
	{
		public ComboTreeBuilder(ComboTree component)
			: base(component)
		{
		}

		public ComboTreeBuilder TextField(string textField)
		{
			base.Component.TextField = textField;
			return this;
		}

		public virtual ComboTreeBuilder Animate(bool animate = true)
		{
			base.Component.Animate = animate;
			return this;
		}

		public virtual ComboTreeBuilder CheckBox(bool checkBox = true)
		{
			base.Component.Checkbox = checkBox;
			return this;
		}

		public virtual ComboTreeBuilder CascadeCheck(bool cascadeCheck = true)
		{
			base.Component.CascadeCheck = cascadeCheck;
			return this;
		}

		public virtual ComboTreeBuilder OnlyLeafCheck(bool onlyLeafCheck = true)
		{
			base.Component.OnlyLeafCheck = onlyLeafCheck;
			return this;
		}

		public virtual ComboTreeBuilder Lines(bool lines = true)
		{
			base.Component.Lines = lines;
			return this;
		}

		public virtual ComboTreeBuilder Data(Action<StringBuilder> action)
		{
			action(base.Component.Data);
			return this;
		}

		public ComboTreeBuilder Ajax(Action<DataSourceBuilder> ajaxAction)
		{
			ajaxAction(new DataSourceBuilder(base.Component.DataSource).Controller("crud").Action("tree"));
			return this;
		}

		public ComboTreeBuilder Events(Action<ComboEventBuilder> clientEventsAction)
		{
			clientEventsAction(new ComboEventBuilder(base.Component.Events));
			return this;
		}
	}
}
