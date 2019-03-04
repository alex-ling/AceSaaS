using Acesoft.Web.UI.Html;
using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class PanelHtmlBuilder<Widget> : EasyUIHtmlBuilder<Widget> where Widget : Panel
	{
		public PanelHtmlBuilder(Widget component)
			: base(component, "div")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Title.HasValue())
			{
				base.Options["title"] = base.Component.Title;
			}
			if (base.Component.IconCls.HasValue())
			{
				base.Options["iconCls"] = base.Component.IconCls;
			}
			if (base.Component.Width.HasValue)
			{
				base.Options["width"] = base.Component.Width;
			}
			if (base.Component.Height.HasValue)
			{
				base.Options["height"] = base.Component.Height;
			}
			if (base.Component.Left.HasValue)
			{
				base.Options["left"] = base.Component.Left;
			}
			if (base.Component.Top.HasValue)
			{
				base.Options["top"] = base.Component.Top;
			}
			if (base.Component.Cls.HasValue())
			{
				base.Options["cls"] = base.Component.Cls;
			}
			if (base.Component.HeaderCls.HasValue())
			{
				base.Options["headerCls"] = base.Component.HeaderCls;
			}
			if (base.Component.BodyCls.HasValue())
			{
				base.Options["bodyCls"] = base.Component.BodyCls;
			}
			if (base.Component.Style != null)
			{
				base.Options["style"] = (object)base.Component.Style;
			}
			if (base.Component.Fit.HasValue)
			{
				base.Options["fit"] = base.Component.Fit;
			}
			if (base.Component.Border.HasValue)
			{
				base.Options["border"] = base.Component.Border;
			}
			if (base.Component.DoSize.HasValue)
			{
				base.Options["doSize"] = base.Component.DoSize;
			}
			if (base.Component.NoHeader.HasValue)
			{
				base.Options["noheader"] = base.Component.NoHeader;
			}
			if (base.Component.Content.HasValue())
			{
				base.Options["content"] = base.Component.Content;
			}
			if (base.Component.HAlign.HasValue)
			{
				base.Options["halign"] = base.Component.HAlign;
			}
			if (base.Component.TitleDirection.HasValue)
			{
				base.Options["titleDirection"] = base.Component.TitleDirection;
			}
			if (base.Component.Collapsible.HasValue)
			{
				base.Options["collapsible"] = base.Component.Collapsible;
			}
			if (base.Component.Minimizable.HasValue)
			{
				base.Options["minimizable"] = base.Component.Minimizable;
			}
			if (base.Component.Maximizable.HasValue)
			{
				base.Options["maximizable"] = base.Component.Maximizable;
			}
			if (base.Component.Closable.HasValue)
			{
				base.Options["closable"] = base.Component.Closable;
			}
			if (base.Component.Header.HasValue())
			{
				base.Options["header"] = base.Component.Header;
			}
			if (base.Component.Footer.HasValue())
			{
				base.Options["footer"] = base.Component.Footer;
			}
			if (base.Component.OpenAnimation.HasValue)
			{
				base.Options["openAnimation"] = base.Component.OpenAnimation;
			}
			if (base.Component.OpenDuration.HasValue)
			{
				base.Options["openDuration"] = base.Component.OpenDuration;
			}
			if (base.Component.CloseAnimation.HasValue)
			{
				base.Options["closeAnimation"] = base.Component.CloseAnimation;
			}
			if (base.Component.CloseDuration.HasValue)
			{
				base.Options["closeDuration"] = base.Component.CloseDuration;
			}
			if (base.Component.Collapsed.HasValue)
			{
				base.Options["collapsed"] = base.Component.Collapsed;
			}
			if (base.Component.Minimized.HasValue)
			{
				base.Options["minimized"] = base.Component.Minimized;
			}
			if (base.Component.Maximized.HasValue)
			{
				base.Options["maximized"] = base.Component.Maximized;
			}
			if (base.Component.Closed.HasValue)
			{
				base.Options["closed"] = base.Component.Closed;
			}
			if (base.Component.Href.HasValue())
			{
				base.Options["href"] = base.Component.Href;
			}
			if (base.Component.Cache.HasValue)
			{
				base.Options["cache"] = base.Component.Cache;
			}
			if (Enumerable.Any<LinkButton>((IEnumerable<LinkButton>)base.Component.Tools))
			{
				base.Options["tools"] = base.Component.Tools;
			}
		}
	}
}
