using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class LayoutItem : Panel
	{
		public Region? Region { get; set; }

		public bool? Split { get; set; }

		public int? MinWidth { get; set; }

		public int? MinHeight { get; set; }

		public int? MaxWidth { get; set; }

		public int? MaxHeight { get; set; }

		public ExpandMode? ExpandMode { get; set; }

		public int? CollapsedSize { get; set; }

		public bool? HideExpandTool { get; set; }

		public bool? HideCollapsedContent { get; set; }

		public string CollapsedContent { get; set; }

		public LayoutItem(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = null;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new LayoutItemHtmlBuilder(this);
		}
	}
}
