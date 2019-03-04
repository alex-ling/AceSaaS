using System;
using System.Collections.Generic;

using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Builder;
using Acesoft.Web.UI.Charts;

namespace Acesoft.Web.UI.Charts
{
	public class EChartBuilder : WidgetBuilder<EChart, EChartBuilder>
	{
		public EChartBuilder(EChart component)
			: base(component)
		{
		}

		public EChartBuilder Option(Action<Option> action)
		{
			action(base.Component.Option);
			return this;
		}

		public EChartBuilder Dataset(bool dataset = true)
		{
			base.Component.Dataset = dataset;
			return this;
		}

		public EChartBuilder Ajax(Action<DataSourceBuilder> ajaxAction)
		{
			ajaxAction(new DataSourceBuilder(base.Component.DataSource).Controller("chart").Action("get"));
			return this;
		}

		public EChartBuilder Events(Action<EChartEventBuilder> clientEventsAction)
		{
			clientEventsAction(new EChartEventBuilder(base.Component.Events));
			return this;
		}
	}

    public class EChartEventBuilder : EventBuilder
    {
        public EChartEventBuilder(IDictionary<string, object> events)
            : base(events)
        {
        }
    }
}
