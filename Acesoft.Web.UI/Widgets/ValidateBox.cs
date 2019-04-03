using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class ValidateBox : WidgetBase
	{
		public static readonly ScriptEvent OnBeforeValidate = new ScriptEvent("onBeforeValidate", "");

		public static readonly ScriptEvent OnValidate = new ScriptEvent("onValidate", "valid");

		public bool? Required { get; set; }

		public string ValidType { get; set; }

		public int? Delay { get; set; }

		public string MissingMessage { get; set; }

		public string InvalidMessage { get; set; }

		public Position? TipPosition { get; set; }

		public int? DeltaX { get; set; }

		public bool? Novalidate { get; set; }

		public bool? Editable { get; set; }

		public bool? Disabled { get; set; }

		public bool? Readonly { get; set; }

		public bool? ValidateOnCreate { get; set; }

		public bool? ValidateOnBlur { get; set; }

		public ValidateBox(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "validatebox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new ValidateBoxHtmlBuilder<ValidateBox>(this);
		}
	}
}
