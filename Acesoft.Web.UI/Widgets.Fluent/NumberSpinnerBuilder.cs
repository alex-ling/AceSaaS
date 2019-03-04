using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class NumberSpinnerBuilder : SpinnerBuilder<NumberSpinner, NumberSpinnerBuilder>
	{
		public NumberSpinnerBuilder(NumberSpinner component)
			: base(component)
		{
		}

		public virtual NumberSpinnerBuilder Precision(int precision)
		{
			base.Component.Precision = precision;
			return this;
		}

		public virtual NumberSpinnerBuilder DecimalSeparator(string decimalSeparator)
		{
			base.Component.DecimalSeparator = decimalSeparator;
			return this;
		}

		public virtual NumberSpinnerBuilder GroupSeparator(string groupSeparator)
		{
			base.Component.GroupSeparator = groupSeparator;
			return this;
		}

		public virtual NumberSpinnerBuilder Prefix(string prefix)
		{
			base.Component.Prefix = prefix;
			return this;
		}

		public virtual NumberSpinnerBuilder Suffix(string suffix)
		{
			base.Component.Suffix = suffix;
			return this;
		}

		public NumberSpinnerBuilder Events(Action<SpinnerEventBuilder> clientEventsAction)
		{
			clientEventsAction(new SpinnerEventBuilder(base.Component.Events));
			return this;
		}
	}
}
