using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class ProgressBarHtmlBuilder : EasyUIHtmlBuilder<ProgressBar>
	{
		public ProgressBarHtmlBuilder(ProgressBar component)
			: base(component, "div")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Width.HasValue)
			{
				base.Options["width"] = base.Component.Width;
			}
			if (base.Component.Height.HasValue)
			{
				base.Options["height"] = base.Component.Height;
			}
			if (base.Component.Value.HasValue)
			{
				base.Options["value"] = base.Component.Value;
			}
			if (base.Component.Text.HasValue())
			{
				base.Options["text"] = base.Component.Text;
			}
		}
	}
}
