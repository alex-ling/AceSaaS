using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class Menu : TreeWidgetBase<MenuItem>
	{
		public static readonly ScriptEvent OnShow = new ScriptEvent("onShow", "");

		public static readonly ScriptEvent OnHide = new ScriptEvent("onHide", "");

		public static readonly ScriptEvent OnClick = new ScriptEvent("onClick", "item");

		public int? ZIndex { get; set; }

		public int? Left { get; set; }

		public int? Top { get; set; }

		public Align? Align { get; set; }

		public int? MinWidth { get; set; }

		public int? ItemHeight { get; set; }

		public int? Duration { get; set; }

		public bool? HideOnUnhover { get; set; }

		public bool? Inline { get; set; }

		public bool? Fit { get; set; }

		public Menu(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "menu";
			base.TagName = "div";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new MenuHtmlBuilder(this);
		}
	}
}
