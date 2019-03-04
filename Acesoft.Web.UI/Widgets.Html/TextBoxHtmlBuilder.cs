using System.Collections.Generic;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class TextBoxHtmlBuilder<Widget> : ValidateBoxHtmlBuilder<Widget> where Widget : TextBox
	{
		public TextBoxHtmlBuilder(Widget component)
			: base(component)
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
			if (base.Component.Cls.HasValue())
			{
				base.Options["cls"] = base.Component.Cls;
			}
			if (base.Component.Prompt.HasValue())
			{
				base.Options["prompt"] = base.Component.Prompt;
			}
			if (base.Component.Value.HasValue())
			{
				base.Options["value"] = base.Component.Value;
			}
			if (base.Component.Type.HasValue)
			{
				base.Options["type"] = base.Component.Type;
			}
			if (base.Component.Label.HasValue())
			{
				base.Options["label"] = base.Component.Label;
			}
			if (base.Component.LabelWidth.HasValue)
			{
				base.Options["labelWidth"] = base.Component.LabelWidth;
			}
			if (base.Component.LabelPosition.HasValue)
			{
				base.Options["labelPosition"] = base.Component.LabelPosition;
			}
			if (base.Component.LabelAlign.HasValue)
			{
				base.Options["labelAlign"] = base.Component.LabelAlign;
			}
			if (base.Component.Multiline.HasValue)
			{
				base.Options["multiline"] = base.Component.Multiline;
			}
			if (Enumerable.Any<LinkButton>((IEnumerable<LinkButton>)base.Component.Icons))
			{
				base.Options["icons"] = base.Component.Icons;
			}
			if (base.Component.IconCls.HasValue())
			{
				base.Options["iconCls"] = base.Component.IconCls;
			}
			if (base.Component.IconAlign.HasValue)
			{
				base.Options["iconAlign"] = base.Component.IconAlign;
			}
			if (base.Component.IconWidth.HasValue)
			{
				base.Options["iconWidth"] = base.Component.IconWidth;
			}
			if (base.Component.ButtonAlign.HasValue)
			{
				base.Options["buttonAlign"] = base.Component.ButtonAlign;
			}
			if (base.Component.ButtonText.HasValue())
			{
				base.Options["buttonText"] = base.Component.ButtonText;
			}
			if (base.Component.ButtonIcon.HasValue())
			{
				base.Options["buttonIcon"] = base.Component.ButtonIcon;
			}
		}
	}
}
