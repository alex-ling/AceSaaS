using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class DataViewHtmlBuilder : WidgetHtmlBuilder<DataView>
	{
		public DataViewHtmlBuilder(DataView component)
			: base(component, "div")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
		}
	}
}
