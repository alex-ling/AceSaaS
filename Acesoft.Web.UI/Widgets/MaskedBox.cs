using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.Text;

namespace Acesoft.Web.UI.Widgets
{
	public class MaskedBox : TextBox
	{
		public string Mask { get; set; }
		public StringBuilder Masks { get; set; }
        public char? PromptChar { get; set; }

        public MaskedBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "maskedbox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new MaskedBoxHtmlBuilder<MaskedBox>(this);
		}
	}
}
