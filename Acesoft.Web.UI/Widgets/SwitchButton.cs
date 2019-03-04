using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class SwitchButton : WidgetBase
	{
		public static readonly ScriptEvent OnChange = new ScriptEvent("onChange", "checked");

		public int? Width
		{
			get;
			set;
		}

		public int? Height
		{
			get;
			set;
		}

		public int? HandleWidth
		{
			get;
			set;
		}

		public bool? Checked
		{
			get;
			set;
		}

		public bool? Disabled
		{
			get;
			set;
		}

		public bool? Readonly
		{
			get;
			set;
		}

		public bool? Reversed
		{
			get;
			set;
		}

		public string OnText
		{
			get;
			set;
		}

		public string OffText
		{
			get;
			set;
		}

		public string HandleText
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public SwitchButton(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "switchbutton";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new SwitchButtonHtmlBuilder(this);
		}
	}
}
