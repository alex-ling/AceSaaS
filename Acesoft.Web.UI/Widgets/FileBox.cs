using Acesoft.Web.UI.Html;
using System;

namespace Acesoft.Web.UI.Widgets
{
	public class FileBox : TextBox
	{
		public string Accept { get; set; }

		public bool? Multiple { get; set; }

		public string Separator { get; set; }

		public FileBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "filebox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			throw new NotImplementedException();
		}
	}
}
