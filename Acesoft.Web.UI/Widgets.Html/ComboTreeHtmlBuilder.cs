namespace Acesoft.Web.UI.Widgets.Html
{
	public class ComboTreeHtmlBuilder<Widget> : ComboHtmlBuilder<Widget> where Widget : ComboTree
	{
		public ComboTreeHtmlBuilder(Widget component)
			: base(component)
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.TextField.HasValue())
			{
				base.Options["textField"] = base.Component.TextField;
			}
			if (base.Component.Animate.HasValue)
			{
				base.Options["animate"] = base.Component.Animate;
			}
			if (base.Component.Checkbox.HasValue)
			{
				base.Options["checkbox"] = base.Component.Checkbox;
			}
			if (base.Component.CascadeCheck.HasValue)
			{
				base.Options["cascadeCheck"] = base.Component.CascadeCheck;
			}
			if (base.Component.OnlyLeafCheck.HasValue)
			{
				base.Options["onlyLeafCheck"] = base.Component.OnlyLeafCheck;
			}
			if (base.Component.Lines.HasValue)
			{
				base.Options["lines"] = base.Component.Lines;
			}
			if (base.Component.Data.Length > 0)
			{
				base.Options["data"] = base.Component.Data;
			}
		}
	}
}
