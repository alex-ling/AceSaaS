using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Charts
{
	public class EChartHtmlBuilder : EasyUIHtmlBuilder<EChart>
	{
		public EChartHtmlBuilder(EChart component)
			: base(component, "div")
		{
			base.RenderType = "aceui";
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			base.Options["option"] = base.Component.Option;
			base.Options["dataset"] = base.Component.Dataset;
		}
	}
}
