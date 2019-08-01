using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class TagBox : ComboBox
	{
		public static readonly ScriptEvent OnTagFormatter = new ScriptEvent("tagFormatter", "value,row");

		public static readonly ScriptEvent OnTagStyle = new ScriptEvent("onTagStyle", "value,row");

		public static readonly ScriptEvent OnClickTag = new ScriptEvent("onClickTag", "value");

		public static readonly ScriptEvent OnBeforeRemoveTag = new ScriptEvent("onBeforeRemoveTag", "value");

		public static readonly ScriptEvent OnRemoveTag = new ScriptEvent("onRemoveTag", "value");

		public TagBox(WidgetFactory ace) : base(ace)
		{
			base.Widget = "tagbox";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new TagBoxHtmlBuilder(this);
		}
	}
}
