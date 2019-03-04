namespace Acesoft.Web.UI.Widgets.Html
{
	public class DataListHtmlBuilder : DataGridHtmlBuilder<DataList>
	{
		public DataListHtmlBuilder(DataList component)
			: base(component, "ul")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Lines.HasValue)
			{
				base.Options["lines"] = base.Component.Lines;
			}
			if (base.Component.ValueField.HasValue())
			{
				base.Options["valueField"] = base.Component.ValueField;
			}
			if (base.Component.TextField.HasValue())
			{
				base.Options["textField"] = base.Component.TextField;
			}
			if (base.Component.GroupField.HasValue())
			{
				base.Options["groupField"] = base.Component.GroupField;
			}
		}
	}
}
