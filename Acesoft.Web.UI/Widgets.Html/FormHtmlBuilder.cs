using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class FormHtmlBuilder<Widget> : WidgetHtmlBuilder<Widget> where Widget : Form
	{
		public FormHtmlBuilder(Widget component)
			: base(component, "form")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Novalidate.HasValue)
			{
				base.Options["novalidate"] = base.Component.Novalidate;
			}
			if (base.Component.Iframe.HasValue)
			{
				base.Options["iframe"] = base.Component.Iframe;
			}
			if (base.Component.Ajax.HasValue)
			{
				base.Options["ajax"] = base.Component.Ajax;
			}
			if (base.Component.Dirty.HasValue)
			{
				base.Options["dirty"] = base.Component.Dirty;
			}
		}
	}
}
