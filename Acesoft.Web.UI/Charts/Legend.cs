using System.Collections.Generic;

namespace Acesoft.Web.UI.Charts
{
	public class Legend : OptionBase<Legend>
	{
		public IList<LegendData> data { get; set; }= new List<LegendData>();

	}
}
