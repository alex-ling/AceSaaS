namespace Acesoft.Web.UI.Widgets.Html
{
	public class PasswordBoxHtmlBuilder : TextBoxHtmlBuilder<PasswordBox>
	{
		public PasswordBoxHtmlBuilder(PasswordBox component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.PasswordChar.HasValue())
			{
				base.Options["passwordChar"] = base.Component.PasswordChar;
			}
			if (base.Component.CheckInterval.HasValue)
			{
				base.Options["checkInterval"] = base.Component.CheckInterval;
			}
			if (base.Component.LastDelay.HasValue)
			{
				base.Options["lastDelay"] = base.Component.LastDelay;
			}
			if (base.Component.Revealed.HasValue)
			{
				base.Options["revealed"] = base.Component.Revealed;
			}
			if (base.Component.ShowEye.HasValue)
			{
				base.Options["showEye"] = base.Component.ShowEye;
			}
		}
	}
}
