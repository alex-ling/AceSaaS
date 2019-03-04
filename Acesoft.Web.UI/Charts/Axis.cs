using System.Collections.Generic;

namespace Acesoft.Web.UI.Charts
{
	public class Axis : OptionBase<Axis>
	{
		public IList<string> Data = new List<string>();

		public Axis TypeCate()
		{
			base.Options["type"] = "category";
			return this;
		}
	}
}
