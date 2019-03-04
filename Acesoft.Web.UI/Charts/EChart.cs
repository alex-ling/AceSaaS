using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;

namespace Acesoft.Web.UI.Charts
{
	public class EChart : WidgetBase, IDataSourceWidget
	{
		public Option Option { get; set; }
		public bool Dataset { get; set; }
        public DataSource DataSource { get; set; }

        public EChart(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "echart";
			Option = new Option();
			DataSource = new DataSource(this);
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new EChartHtmlBuilder(this);
		}
	}
}
