using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class LinkButtonBuilder : LinkButtonBuilder<LinkButton, LinkButtonBuilder>
	{
		public LinkButtonBuilder(LinkButton component)
			: base(component)
		{
		}

		public LinkButtonBuilder Events(Action<ButtonEventBuilder> clientEventsAction)
		{
			clientEventsAction(new ButtonEventBuilder(base.Component.Events));
			return this;
		}
	}
	public class LinkButtonBuilder<Widget, Builder> : WidgetBuilder<Widget, Builder> where Widget : LinkButton where Builder : WidgetBuilder<Widget, Builder>
	{
		public LinkButtonBuilder(Widget component)
			: base(component)
		{
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

		public new virtual Builder Disabled(bool disabled = true)
		{
			base.Component.Disabled = disabled;
			return this as Builder;
		}

		public virtual Builder Toggle(bool toggle = true)
		{
			base.Component.Toggle = toggle;
			return this as Builder;
		}

		public virtual Builder Selected(bool selected = true)
		{
			base.Component.Selected = selected;
			return this as Builder;
		}

		public virtual Builder Group(string group)
		{
			base.Component.Group = group;
			return this as Builder;
		}

		public virtual Builder Plain(bool plain = true)
		{
			base.Component.Plain = plain;
			return this as Builder;
		}

		public virtual Builder Text(string text)
		{
			base.Component.Text = text;
			return this as Builder;
		}

		public virtual Builder IconCls(string iconCls)
		{
			base.Component.IconCls = iconCls;
			return this as Builder;
		}

		public virtual Builder IconAlign(Align align)
		{
			base.Component.IconAlign = align;
			return this as Builder;
		}

		public virtual Builder Size(Size size)
		{
			base.Component.Size = size;
			return this as Builder;
		}

		public virtual Builder Click(string handler)
		{
			base.Component.Events["onClick"] = new ScriptHandler
			{
				Handler = "function(){" + handler + "}"
			};
			return this as Builder;
		}
	}
}
