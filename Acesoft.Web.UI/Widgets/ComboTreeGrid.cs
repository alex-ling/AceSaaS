using Acesoft.Web.UI.Html;
using System;

namespace Acesoft.Web.UI.Widgets
{
	public class ComboTreeGrid : TreeGrid
	{
		public string TextField
		{
			get;
			set;
		}

		public bool? LimitToGrid
		{
			get;
			set;
		}

		public ComboTreeGrid(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "combotreegrid";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			throw new NotImplementedException();
		}
	}
}
