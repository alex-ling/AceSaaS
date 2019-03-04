using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class Search : Form
	{
		public bool? Button
		{
			get;
			set;
		}

		public IList<IHtmlContent> Tools
		{
			get;
			set;
		}

		public Search(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "search";
			Button = true;
			Tools = new List<IHtmlContent>();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new SearchHtmlBuilder(this);
		}
	}
}
