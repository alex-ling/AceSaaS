using Acesoft.Web.UI.Builder;
using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class ValidateBoxBuilder : ValidateBoxBuilder<ValidateBox, ValidateBoxBuilder>
	{
		public ValidateBoxBuilder(ValidateBox component)
			: base(component)
		{
		}

		public ValidateBoxBuilder Events(Action<ValidateBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new ValidateBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
	public class ValidateBoxBuilder<Widget, Builder> : WidgetBuilder<Widget, Builder> where Widget : ValidateBox where Builder : WidgetBuilder<Widget, Builder>
	{
		public ValidateBoxBuilder(Widget component)
			: base(component)
		{
		}

		public virtual Builder Required(bool required = true)
		{
			base.Component.Required = required;
			return this as Builder;
		}

		public virtual Builder ValidType(string validType)
		{
			base.Component.ValidType = validType;
			return this as Builder;
		}

		public virtual Builder Delay(int eelay)
		{
			base.Component.Delay = eelay;
			return this as Builder;
		}

		public virtual Builder MissingMessage(string missingMessage)
		{
			base.Component.MissingMessage = missingMessage;
			return this as Builder;
		}

		public virtual Builder InvalidMessage(string invalidMessage)
		{
			base.Component.InvalidMessage = invalidMessage;
			return this as Builder;
		}

		public virtual Builder TipPosition(Position position)
		{
			base.Component.TipPosition = position;
			return this as Builder;
		}

		public virtual Builder DeltaX(int deltaX)
		{
			base.Component.DeltaX = deltaX;
			return this as Builder;
		}

		public virtual Builder Novalidate(bool novalidate = true)
		{
			base.Component.Novalidate = novalidate;
			return this as Builder;
		}

		public virtual Builder Editable(bool editable = true)
		{
			base.Component.Editable = editable;
			return this as Builder;
		}

		public new virtual Builder Disabled(bool disabled = true)
		{
			base.Component.Disabled = disabled;
			return this as Builder;
		}

		public virtual Builder Readonly(bool @readonly = true)
		{
			base.Component.Readonly = @readonly;
			return this as Builder;
		}

		public virtual Builder ValidateOnCreate(bool validateOnCreate = true)
		{
			base.Component.ValidateOnCreate = validateOnCreate;
			return this as Builder;
		}

		public virtual Builder ValidateOnBlur(bool validateOnBlur = true)
		{
			base.Component.ValidateOnBlur = validateOnBlur;
			return this as Builder;
		}
	}
}
