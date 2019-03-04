using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class MonthBox : ComboBox, IDataBind
	{
		public MonthBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "combobox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new ComboBoxHtmlBuilder<ComboBox>(this);
		}

		public void DataBind()
		{
			for (int i = 1; i <= 12; i++)
			{
				base.Data.Add(new ComboItem(i.ToString("00")));
			}
		}
	}
}
