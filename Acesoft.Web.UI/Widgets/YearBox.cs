using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class YearBox : ComboBox, IDataBind
	{
		public int Start
		{
			get;
			set;
		}

		public int End
		{
			get;
			set;
		}

		public YearBox(WidgetFactory ace)
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
			for (int i = Start; i <= End; i++)
			{
				base.Data.Add(new ComboItem($"{i}"));
			}
		}
	}
}
