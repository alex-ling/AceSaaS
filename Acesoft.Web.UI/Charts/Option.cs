using System.Collections.Generic;

namespace Acesoft.Web.UI.Charts
{
	public class Option : OptionBase<Option>
	{
		public Title Title
		{
			get;
			set;
		}

		public Legend Legend
		{
			get;
			set;
		}

		public Axis XAxis
		{
			get;
			set;
		}

		public Axis YAxis
		{
			get;
			set;
		}

		public IList<Series> Series
		{
			get;
			set;
		} = new List<Series>();


		public Title AddTitle()
		{
			Title = new Title();
			return Title;
		}

		public Legend AddLegend()
		{
			Legend = new Legend();
			return Legend;
		}

		public Axis AddXAxis()
		{
			XAxis = new Axis();
			return XAxis;
		}

		public Axis AddYAxis()
		{
			YAxis = new Axis();
			return YAxis;
		}

		public Series AddSeries(ChartType type)
		{
			Series series = new Series
			{
				Type = type
			};
			Series.Add(series);
			return series;
		}
	}
}
