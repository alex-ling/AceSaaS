using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class DataGridColumnHtmlBuilder : EasyUIHtmlBuilder<DataGridColumn>
	{
		public DataGridColumnHtmlBuilder(DataGridColumn component)
			: base(component, "th")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Title.HasValue())
			{
				base.Options["title"] = base.Component.Title;
			}
			if (base.Component.Field.HasValue())
			{
				base.Options["field"] = base.Component.Field;
			}
			if (base.Component.Width.HasValue)
			{
				base.Options["width"] = base.Component.Width;
			}
			if (base.Component.Rowspan.HasValue)
			{
				base.Options["rowspan"] = base.Component.Rowspan;
			}
			if (base.Component.Colspan.HasValue)
			{
				base.Options["colspan"] = base.Component.Colspan;
			}
			if (base.Component.Align.HasValue)
			{
				base.Options["align"] = base.Component.Align;
			}
			if (base.Component.Halign.HasValue)
			{
				base.Options["halign"] = base.Component.Halign;
			}
			if (base.Component.Sortable.HasValue)
			{
				base.Options["sortable"] = base.Component.Sortable;
			}
			if (base.Component.Order.HasValue)
			{
				base.Options["order"] = base.Component.Order;
			}
			if (base.Component.Resizable.HasValue)
			{
				base.Options["resizable"] = base.Component.Resizable;
			}
			if (base.Component.Fixed.HasValue)
			{
				base.Options["fixed"] = base.Component.Fixed;
			}
			if (base.Component.Hidden.HasValue)
			{
				base.Options["hidden"] = base.Component.Hidden;
			}
			if (base.Component.Checkbox.HasValue)
			{
				base.Options["checkbox"] = base.Component.Checkbox;
			}
			if (base.Component.Editor.HasValue())
			{
				base.Options["editor"] = base.Component.Editor;
			}
			if (base.Component.Merged.HasValue)
			{
				base.Options["merged"] = base.Component.Merged;
			}
			if (base.Component.Format.HasValue())
			{
				base.Options["format"] = base.Component.Format;
				ScriptEvent.Regist(base.Component.Events, DataGridColumn.Formatter, "AX.gridFmt");
			}
		}
	}
}
