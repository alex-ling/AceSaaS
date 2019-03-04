using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class LinkButtonHtmlBuilder<Widget> : EasyUIHtmlBuilder<Widget> where Widget : LinkButton
	{
		public LinkButtonHtmlBuilder(Widget component)
			: base(component, "a")
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
			if (base.Component.Disabled.HasValue)
			{
				base.Options["disabled"] = base.Component.Disabled;
			}
			if (base.Component.Toggle.HasValue)
			{
				base.Options["toggle"] = base.Component.Toggle;
			}
			if (base.Component.Selected.HasValue)
			{
				base.Options["selected"] = base.Component.Selected;
			}
			if (base.Component.Group.HasValue())
			{
				base.Options["group"] = base.Component.Group;
			}
			if (base.Component.Plain.HasValue)
			{
				base.Options["plain"] = base.Component.Plain;
			}
			if (base.Component.Text.HasValue())
			{
				base.Options["text"] = base.Component.Text;
			}
			if (base.Component.IconCls.HasValue())
			{
				base.Options["iconCls"] = base.Component.IconCls;
			}
			if (base.Component.IconAlign.HasValue)
			{
				base.Options["iconAlign"] = base.Component.IconAlign;
			}
			if (base.Component.Size.HasValue)
			{
				base.Options["size"] = base.Component.Size;
			}
		}
	}
}
