using System.Collections.Generic;

namespace Acesoft.Web.UI.Charts
{
	public class Series : OptionBase<Series>
	{
		public ChartType Type
		{
			get;
			set;
		}

		public IList<SeriesData> Data
		{
			get;
			set;
		} = new List<SeriesData>();


		public Series Name(string name)
		{
			base.Options["name"] = name;
			return this;
		}
	}
}
