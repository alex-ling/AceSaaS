using Acesoft.Web.UI.Builder;
using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class PanelBuilder : PanelBuilder<Panel, PanelBuilder>
	{
		public PanelBuilder(Panel component)
			: base(component)
		{
		}

		public PanelBuilder Events(Action<PanelEventBuilder> clientEventsAction)
		{
			clientEventsAction(new PanelEventBuilder(base.Component.Events));
			return this;
		}
	}
	public abstract class PanelBuilder<Widget, Builder> : ContentBuilder<Widget, Builder> where Widget : Panel where Builder : WidgetBuilder<Widget, Builder>
	{
		public PanelBuilder(Widget component)
			: base(component)
		{
		}

		public override Builder Title(string title)
		{
			base.Component.Title = title;
			return this as Builder;
		}

		public virtual Builder IconCls(string iconCls)
		{
			base.Component.IconCls = iconCls;
			return this as Builder;
		}

		public virtual Builder Width(int width)
		{
			base.Component.Width = width;
			return this as Builder;
		}

		public virtual Builder Height(int height)
		{
			base.Component.Height = height;
			return this as Builder;
		}

		public virtual Builder Left(int left)
		{
			base.Component.Left = left;
			return this as Builder;
		}

		public virtual Builder Top(int top)
		{
			base.Component.Top = top;
			return this as Builder;
		}

		public virtual Builder Cls(string cls)
		{
			base.Component.Cls = cls;
			return this as Builder;
		}

		public virtual Builder HeaderCls(string headerCls)
		{
			base.Component.HeaderCls = headerCls;
			return this as Builder;
		}

		public virtual Builder BodyCls(string bodyCls)
		{
			base.Component.BodyCls = bodyCls;
			return this as Builder;
		}

		public virtual Builder Style(object style)
		{
			base.Component.Style = style;
			return this as Builder;
		}

		public virtual Builder Fit(bool fit = true)
		{
			base.Component.Fit = fit;
			return this as Builder;
		}

		public virtual Builder Border(bool border = true)
		{
			base.Component.Border = border;
			return this as Builder;
		}

		public virtual Builder DoSize(bool doSize = true)
		{
			base.Component.DoSize = doSize;
			return this as Builder;
		}

		public virtual Builder Noheader(bool noheader = true)
		{
			base.Component.NoHeader = noheader;
			return this as Builder;
		}

		public virtual Builder HAlign(Align align)
		{
			base.Component.HAlign = align;
			return this as Builder;
		}

		public virtual Builder TitleDirection(Direction direction)
		{
			base.Component.TitleDirection = direction;
			return this as Builder;
		}

		public virtual Builder Collapsible(bool collapsible = true)
		{
			base.Component.Collapsible = collapsible;
			return this as Builder;
		}

		public virtual Builder Minimizable(bool minimizable = true)
		{
			base.Component.Minimizable = minimizable;
			return this as Builder;
		}

		public virtual Builder Maximizable(bool maximizable = true)
		{
			base.Component.Maximizable = maximizable;
			return this as Builder;
		}

		public virtual Builder Closable(bool closable = true)
		{
			base.Component.Closable = closable;
			return this as Builder;
		}

		public virtual Builder Tools(Action<IList<LinkButton>> toolsAction)
		{
			toolsAction(base.Component.Tools);
			return this as Builder;
		}

		public virtual Builder Header(string header)
		{
			base.Component.Header = header;
			return this as Builder;
		}

		public virtual Builder Footer(string footer)
		{
			base.Component.Footer = footer;
			return this as Builder;
		}

		public virtual Builder OpenAnimation(Animation animation)
		{
			base.Component.OpenAnimation = animation;
			return this as Builder;
		}

		public virtual Builder OpenDuration(int duration)
		{
			base.Component.OpenDuration = duration;
			return this as Builder;
		}

		public virtual Builder CloseAnimation(Animation animation)
		{
			base.Component.CloseAnimation = animation;
			return this as Builder;
		}

		public virtual Builder CloseDuration(int duration)
		{
			base.Component.CloseDuration = duration;
			return this as Builder;
		}

		public virtual Builder Collapsed(bool collapsed = true)
		{
			base.Component.Collapsed = collapsed;
			return this as Builder;
		}

		public virtual Builder Minimized(bool minimized = true)
		{
			base.Component.Minimized = minimized;
			return this as Builder;
		}

		public virtual Builder Maximized(bool maximized = true)
		{
			base.Component.Maximized = maximized;
			return this as Builder;
		}

		public virtual Builder Closed(bool closed = true)
		{
			base.Component.Closed = closed;
			return this as Builder;
		}

		public virtual Builder Href(string href)
		{
			base.Component.Href = href;
			return this as Builder;
		}

		public virtual Builder Cache(bool cache = true)
		{
			base.Component.Cache = cache;
			return this as Builder;
		}
	}
}
