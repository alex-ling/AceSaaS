using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class Accordion : ListWidgetBase<AccordionItem>
	{
		public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "title,index");

		public static readonly ScriptEvent OnUnselect = new ScriptEvent("onUnselect", "title,index");

		public static readonly ScriptEvent OnAdd = new ScriptEvent("onAdd", "title,index");

		public static readonly ScriptEvent OnBeforeRemove = new ScriptEvent("onBeforeRemove", "title,index");

		public static readonly ScriptEvent OnRemove = new ScriptEvent("onRemove", "title,index");

		public bool? Fit
		{
			get;
			set;
		}

		public bool? Border
		{
			get;
			set;
		}

		public bool? Animate
		{
			get;
			set;
		}

		public bool? Multiple
		{
			get;
			set;
		}

		public int? Selected
		{
			get;
			set;
		}

		public Align? HAlign
		{
			get;
			set;
		}

		public Accordion(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "accordion";
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new AccordionHtmlBuilder(this);
		}
	}
}
