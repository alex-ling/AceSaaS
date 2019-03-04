using Acesoft.Web.UI.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class ValidateBoxHtmlBuilder<Widget> : EasyUIHtmlBuilder<Widget> where Widget : ValidateBox
	{
		public ValidateBoxHtmlBuilder(Widget component)
			: base(component, "input")
		{
			base.RenderMode = TagRenderMode.SelfClosing;
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Required.HasValue)
			{
				base.Options["required"] = base.Component.Required;
			}
			if (base.Component.ValidType.HasValue())
			{
				base.Options["validType"] = base.Component.ValidType;
			}
			if (base.Component.Delay.HasValue)
			{
				base.Options["delay"] = base.Component.Delay;
			}
			if (base.Component.MissingMessage.HasValue())
			{
				base.Options["missingMessage"] = base.Component.MissingMessage;
			}
			if (base.Component.InvalidMessage.HasValue())
			{
				base.Options["invalidMessage"] = base.Component.InvalidMessage;
			}
			if (base.Component.TipPosition.HasValue)
			{
				base.Options["tipPosition"] = base.Component.TipPosition;
			}
			if (base.Component.DeltaX.HasValue)
			{
				base.Options["deltaX"] = base.Component.DeltaX;
			}
			if (base.Component.Novalidate.HasValue)
			{
				base.Options["novalidate"] = base.Component.Novalidate;
			}
			if (base.Component.Editable.HasValue)
			{
				base.Options["editable"] = base.Component.Editable;
			}
			if (base.Component.Disabled.HasValue)
			{
				base.Options["disabled"] = base.Component.Disabled;
			}
			if (base.Component.Readonly.HasValue)
			{
				base.Options["readonly"] = base.Component.Readonly;
			}
			if (base.Component.ValidateOnCreate.HasValue)
			{
				base.Options["validateOnCreate"] = base.Component.ValidateOnCreate;
			}
			if (base.Component.ValidateOnBlur.HasValue)
			{
				base.Options["validateOnBlur"] = base.Component.ValidateOnBlur;
			}
		}
	}
}
