using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.IO;

namespace Acesoft.Web.UI.Widgets
{
	public class View : Form
	{
		public View(WidgetFactory ace)
			: base(ace)
		{
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new FormHtmlBuilder<View>(this);
		}

		public override void WriteInitScript(TextWriter writer)
		{
			base.WriteInitScript(writer);
			writer.Write("function onSubmit(cb){AX.formSubmit('#" + base.Id + "',cb);}");
		}
	}
}
