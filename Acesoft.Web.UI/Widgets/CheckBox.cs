using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class CheckBox : WidgetBase
	{
		public string Text
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public string Group
		{
			get;
			set;
		}

		public bool Checked
		{
			get;
			set;
		}

		public InputType Type
		{
			get;
			set;
		}

		public CheckBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "checkbox";
			Value = "1";
			Type = InputType.checkbox;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new CheckBoxHtmlBuilder(this);
		}
	}
}
