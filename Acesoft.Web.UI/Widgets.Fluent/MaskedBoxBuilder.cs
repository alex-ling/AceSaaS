using System;
using System.Text;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class MaskedBoxBuilder : TextBoxBuilder<MaskedBox, MaskedBoxBuilder>
	{
		public MaskedBoxBuilder(MaskedBox component)
			: base(component)
		{
		}

		public virtual MaskedBoxBuilder Mask(string mask)
		{
			base.Component.Mask = mask;
			return this;
		}

		public virtual MaskedBoxBuilder Masks(string masks)
		{
			base.Component.Masks = new StringBuilder(masks);
			return this;
		}

		public virtual MaskedBoxBuilder Suffix(char promptChar)
		{
			base.Component.PromptChar = promptChar;
			return this;
		}

		public MaskedBoxBuilder Events(Action<TextBoxEventBuilder> clientEventsAction)
		{
			clientEventsAction(new TextBoxEventBuilder(base.Component.Events));
			return this;
		}
	}
}
