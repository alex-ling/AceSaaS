using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class PasswordBox : TextBox
	{
		public string PasswordChar
		{
			get;
			set;
		}

		public int? CheckInterval
		{
			get;
			set;
		}

		public int? LastDelay
		{
			get;
			set;
		}

		public bool? Revealed
		{
			get;
			set;
		}

		public bool? ShowEye
		{
			get;
			set;
		}

		public PasswordBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "passwordbox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new PasswordBoxHtmlBuilder(this);
		}
	}
}
