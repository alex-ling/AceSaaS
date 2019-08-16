using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class TextBox : ValidateBox
	{
		public static readonly ScriptEvent OnChange = new ScriptEvent("onChange", "newValue,oldValue");

		public static readonly ScriptEvent OnResize = new ScriptEvent("onResize", "width,height");

		public static readonly ScriptEvent OnClickButton = new ScriptEvent("onClickButton", "");

		public static readonly ScriptEvent OnClickIcon = new ScriptEvent("onClickIcon", "index");

		public int? Width { get; set; }

		public int? Height { get; set; }

		public string Cls { get; set; }

		public string Prompt { get; set; }

		public string Value { get; set; }

		public InputType? Type { get; set; }

		public string Label { get; set; }

		public int? LabelWidth { get; set; }

		public Position? LabelPosition { get; set; }

		public Align? LabelAlign { get; set; }

		public bool? Multiline { get; set; }

		public List<LinkButton> Icons { get; set; }

		public string IconCls { get; set; }

		public Align? IconAlign { get; set; }

		public int? IconWidth { get; set; }

		public Align? ButtonAlign { get; set; }

		public string ButtonText { get; set; }

		public string ButtonIcon { get; set; }

		public TextBox(WidgetFactory ace)
			: base(ace)
		{
			base.NoRenderLabel = true;
			base.Widget = "textbox";
			Icons = new List<LinkButton>();
			Height = 26;
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new TextBoxHtmlBuilder<TextBox>(this);
		}
	}
}
