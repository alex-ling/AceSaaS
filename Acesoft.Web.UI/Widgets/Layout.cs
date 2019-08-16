using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class Layout : ListWidgetBase<LayoutItem>
	{
		public static readonly ScriptEvent OnCollapse = new ScriptEvent("onCollapse", "region");

		public static readonly ScriptEvent OnExpand = new ScriptEvent("onExpand", "region");

		public static readonly ScriptEvent OnAdd = new ScriptEvent("onAdd", "region");

		public static readonly ScriptEvent OnRemove = new ScriptEvent("onRemove", "region");

		public bool? Fit { get; set; }

		public Layout(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "layout";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new LayoutHtmlBuilder(this);
		}
	}
}
