using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class MenuItem : TreeNodeWidgetBase<Menu, MenuItem>
	{
		public static readonly ScriptEvent OnClick = new ScriptEvent("onClick", "");

		public string IconCls { get; set; }

		public string Href { get; set; }

		public bool? Disabled { get; set; }

		public MenuItem(WidgetFactory ace, Menu menu)
			: base(ace, menu)
		{
			base.Widget = null;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new MenuItemHtmlBuilder(this);
		}
	}
}
