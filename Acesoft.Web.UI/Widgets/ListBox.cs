using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class ListBox : HiddenBox
	{
        public string SelectUrl { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Prompt { get; set; }

        public ListBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "listbox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new ListBoxHtmlBuilder<ListBox>(this);
		}
	}
}
