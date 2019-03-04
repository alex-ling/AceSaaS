using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class LayoutItemBuilder : PanelBuilder<LayoutItem, LayoutItemBuilder>
	{
		public LayoutItemBuilder(LayoutItem component)
			: base(component)
		{
		}

		public virtual LayoutItemBuilder Region(Region region)
		{
			base.Component.Region = region;
			return this;
		}

		public virtual LayoutItemBuilder Split(bool split = true)
		{
			base.Component.Split = split;
			return this;
		}

		public virtual LayoutItemBuilder MinWidth(int minWidth)
		{
			base.Component.MinWidth = minWidth;
			return this;
		}

		public virtual LayoutItemBuilder MinHeight(int minHeight)
		{
			base.Component.MinHeight = minHeight;
			return this;
		}

		public virtual LayoutItemBuilder MaxWidth(int maxWidth)
		{
			base.Component.MaxWidth = maxWidth;
			return this;
		}

		public virtual LayoutItemBuilder MaxHeight(int maxHeight)
		{
			base.Component.MaxHeight = maxHeight;
			return this;
		}

		public virtual LayoutItemBuilder ExpandMode(ExpandMode expandMode)
		{
			base.Component.ExpandMode = expandMode;
			return this;
		}

		public virtual LayoutItemBuilder CollapsedSize(int collapsedSize)
		{
			base.Component.CollapsedSize = collapsedSize;
			return this;
		}

		public virtual LayoutItemBuilder HideExpandTool(bool hideExpandTool = true)
		{
			base.Component.HideExpandTool = hideExpandTool;
			return this;
		}

		public virtual LayoutItemBuilder HideCollapsedContent(bool hideCollapsedContent = true)
		{
			base.Component.HideCollapsedContent = hideCollapsedContent;
			return this;
		}

		public virtual LayoutItemBuilder CollapsedContent(string collapsedContent)
		{
			base.Component.CollapsedContent = collapsedContent;
			return this;
		}

		public LayoutItemBuilder Events(Action<PanelEventBuilder> clientEventsAction)
		{
			clientEventsAction(new PanelEventBuilder(base.Component.Events));
			return this;
		}
	}
}
