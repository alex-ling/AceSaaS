using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class TextBoxBuilder : TextBoxBuilder<TextBox, TextBoxBuilder>
	{
		public TextBoxBuilder(TextBox component)
			: base(component)
		{
		}

		public TextBoxBuilder Events(Action<TextBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new TextBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
	public abstract class TextBoxBuilder<Widget, Builder> : ValidateBoxBuilder<Widget, Builder> where Widget : TextBox where Builder : TextBoxBuilder<Widget, Builder>
	{
		public TextBoxBuilder(Widget component)
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

		public virtual Builder Cls(string cls)
		{
			base.Component.Cls = cls;
			return this as Builder;
		}

		public virtual Builder Prompt(string prompt)
		{
			base.Component.Prompt = prompt;
			return this as Builder;
		}

		public virtual Builder Value(string value)
		{
			base.Component.Value = value;
			return this as Builder;
		}

		public virtual Builder Type(InputType type)
		{
			base.Component.Type = type;
			return this as Builder;
		}

		public virtual Builder Label(string label)
		{
			base.Component.Label = label;
			return this as Builder;
		}

		public virtual Builder LabelWidth(int labelWidth)
		{
			base.Component.LabelWidth = labelWidth;
			return this as Builder;
		}

		public virtual Builder LabelPosition(Position position)
		{
			base.Component.LabelPosition = position;
			return this as Builder;
		}

		public virtual Builder LabelAlign(Align align)
		{
			base.Component.LabelAlign = align;
			return this as Builder;
		}

		public virtual Builder Multiline(bool multiline = true)
		{
			base.Component.Multiline = multiline;
			return this as Builder;
		}

		public virtual Builder Icons(Action<IList<LinkButton>> iconsAction)
		{
			iconsAction(base.Component.Icons);
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

		public virtual Builder IconWidth(int iconWidth)
		{
			base.Component.IconWidth = iconWidth;
			return this as Builder;
		}

		public virtual Builder ButtonAlign(Align align)
		{
			base.Component.ButtonAlign = align;
			return this as Builder;
		}

		public virtual Builder ButtonText(string buttonText)
		{
			base.Component.ButtonText = buttonText;
			return this as Builder;
		}

		public virtual Builder ButtonIcon(string buttonIcon)
		{
			base.Component.ButtonIcon = buttonIcon;
			return this as Builder;
		}
	}
}
