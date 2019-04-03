using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class Combo : TextBox
	{
		public static readonly ScriptEvent OnShowPanel = new ScriptEvent("onShowPanel", "");

		public static readonly ScriptEvent OnHidePanel = new ScriptEvent("onHidePanel", "");

		public int? PanelWidth { get; set; }

		public int? PanelHeight { get; set; }

		public int? PanelMinWidth { get; set; }

		public int? PanelMinHeight { get; set; }

		public int? PanelMaxWidth { get; set; }

		public int? PanelMaxHeight { get; set; }

		public Align? PanelAlign { get; set; }

		public bool? Multiple { get; set; }

		public bool? Multivalue { get; set; }

		public bool? Reversed { get; set; }

		public bool? SelectOnNavigation { get; set; }

		public string Separator { get; set; }

		public bool? HasDownArrow { get; set; }

		public Combo(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "combo";
            this.Editable = false;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new ComboHtmlBuilder<Combo>(this);
		}
	}
}
