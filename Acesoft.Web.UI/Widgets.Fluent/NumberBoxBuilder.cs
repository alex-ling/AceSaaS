using System;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class NumberBoxBuilder : TextBoxBuilder<NumberBox, NumberBoxBuilder>
	{
		public NumberBoxBuilder(NumberBox component)
			: base(component)
		{
		}

		public virtual NumberBoxBuilder Min(int min)
		{
			base.Component.Min = min;
			return this;
		}

		public virtual NumberBoxBuilder Max(int max)
		{
			base.Component.Max = max;
			return this;
		}

		public virtual NumberBoxBuilder Precision(int precision)
		{
			base.Component.Precision = precision;
			return this;
		}

		public virtual NumberBoxBuilder DecimalSeparator(string decimalSeparator)
		{
			base.Component.DecimalSeparator = decimalSeparator;
			return this;
		}

		public virtual NumberBoxBuilder GroupSeparator(string groupSeparator)
		{
			base.Component.GroupSeparator = groupSeparator;
			return this;
		}

		public virtual NumberBoxBuilder Prefix(string prefix)
		{
			base.Component.Prefix = prefix;
			return this;
		}

		public virtual NumberBoxBuilder Suffix(string suffix)
		{
			base.Component.Suffix = suffix;
			return this;
		}

		public NumberBoxBuilder Events(Action<NumberBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new NumberBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
}
