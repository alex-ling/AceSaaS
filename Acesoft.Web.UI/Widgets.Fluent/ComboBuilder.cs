using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ComboBuilder : ComboBuilder<Combo, ComboBuilder>
	{
		public ComboBuilder(Combo component)
			: base(component)
		{
		}

		public ComboBuilder Events(Action<ComboEventBuilder> clientEventsAction)
		{
			clientEventsAction(new ComboEventBuilder(base.Component.Events));
			return this;
		}
	}
	public class ComboBuilder<Widget, Builder> : TextBoxBuilder<Widget, Builder> where Widget : Combo where Builder : ComboBuilder<Widget, Builder>
	{
		public ComboBuilder(Widget component)
			: base(component)
		{
		}

		public virtual Builder PanelWidth(int panelWidth)
		{
			base.Component.PanelWidth = panelWidth;
			return this as Builder;
		}

		public virtual Builder PanelHeight(int panelHeight)
		{
			base.Component.PanelHeight = panelHeight;
			return this as Builder;
		}

		public virtual Builder PanelMinWidth(int panelMinWidth)
		{
			base.Component.PanelMinWidth = panelMinWidth;
			return this as Builder;
		}

		public virtual Builder PanelMinHeight(int panelMinHeight)
		{
			base.Component.PanelMinHeight = panelMinHeight;
			return this as Builder;
		}

		public virtual Builder PanelMaxWidth(int panelMaxWidth)
		{
			base.Component.PanelMaxWidth = panelMaxWidth;
			return this as Builder;
		}

		public virtual Builder PanelMaxHeight(int panelMaxHeight)
		{
			base.Component.PanelMaxHeight = panelMaxHeight;
			return this as Builder;
		}

		public virtual Builder PanelAlign(Align align)
		{
			base.Component.PanelAlign = align;
			return this as Builder;
		}

		public virtual Builder Multiple(bool multiple = true)
		{
			base.Component.Multiple = multiple;
			return this as Builder;
		}

		public virtual Builder Multivalue(bool multivalue = true)
		{
			base.Component.Multivalue = multivalue;
			return this as Builder;
		}

		public virtual Builder Reversed(bool reversed = true)
		{
			base.Component.Reversed = reversed;
			return this as Builder;
		}

		public virtual Builder SelectOnNavigation(bool selectOnNavigation = true)
		{
			base.Component.SelectOnNavigation = selectOnNavigation;
			return this as Builder;
		}

		public virtual Builder Separator(string separator)
		{
			base.Component.Separator = separator;
			return this as Builder;
		}

		public virtual Builder HasDownArrow(bool aasDownArrow = true)
		{
			base.Component.HasDownArrow = aasDownArrow;
			return this as Builder;
		}
	}
}
