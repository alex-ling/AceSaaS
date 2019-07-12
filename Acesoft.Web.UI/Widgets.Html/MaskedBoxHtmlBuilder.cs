namespace Acesoft.Web.UI.Widgets.Html
{
	public class MaskedBoxHtmlBuilder<Widget> : TextBoxHtmlBuilder<Widget> where Widget : MaskedBox
	{
		public MaskedBoxHtmlBuilder(Widget component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();

			if (base.Component.Mask.HasValue())
			{
				base.Options["mask"] = base.Component.Mask;
			}
			if (base.Component.Masks != null)
			{
				base.Options["masks"] = base.Component.Masks;
			}
			if (base.Component.PromptChar.HasValue)
			{
				base.Options["promptChar"] = base.Component.PromptChar;
			}
		}
	}
}
