using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class LinkButton : WidgetBase
	{
		public static readonly ScriptEvent OnClick = new ScriptEvent("onClick", "");

		public int? Width { get; set; }

		public int? Height { get; set; }

		public bool? Disabled { get; set; }

		public bool? Toggle { get; set; }

		public bool? Selected { get; set; }

		public string Group { get; set; }

		public bool? Plain { get; set; }

		public string Text { get; set; }

		public string IconCls { get; set; }

		public Align? IconAlign { get; set; }

		public Size? Size { get; set; }

		public LinkButton(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "linkbutton";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new LinkButtonHtmlBuilder<LinkButton>(this);
		}
	}
}
