using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class Tabs : ListWidgetBase<TabItem>
	{
		public static readonly ScriptEvent OnLoad = new ScriptEvent("onLoad", "panel");

		public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "title,index");

		public static readonly ScriptEvent OnUnselect = new ScriptEvent("onUnselect", "title,index");

		public static readonly ScriptEvent OnBeforeClose = new ScriptEvent("onBeforeClose", "title,index");

		public static readonly ScriptEvent OnClose = new ScriptEvent("onClose", "title,index");

		public static readonly ScriptEvent OnAdd = new ScriptEvent("onAdd", "title,index");

		public static readonly ScriptEvent OnUpdate = new ScriptEvent("onUpdate", "title,index");

		public static readonly ScriptEvent OnContextMenu = new ScriptEvent("onContextMenu", "title,index");

		public int? Width
		{
			get;
			set;
		}

		public int? Height
		{
			get;
			set;
		}

		public bool? Plain
		{
			get;
			set;
		}

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

		public int? ScrollIncrement
		{
			get;
			set;
		}

		public int? ScrollDuration
		{
			get;
			set;
		}

		public List<LinkButton> Tools
		{
			get;
			set;
		}

		public Position? ToolPosition
		{
			get;
			set;
		}

		public Position? TabPosition
		{
			get;
			set;
		}

		public int? HeaderWidth
		{
			get;
			set;
		}

		public int? TabWidth
		{
			get;
			set;
		}

		public int? TabHeight
		{
			get;
			set;
		}

		public int? Selected
		{
			get;
			set;
		}

		public bool? ShowHeader
		{
			get;
			set;
		}

		public bool? Justified
		{
			get;
			set;
		}

		public bool? Narrow
		{
			get;
			set;
		}

		public bool? Pill
		{
			get;
			set;
		}

		public Tabs(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "tabs";
			Tools = new List<LinkButton>();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new TabsHtmlBuilder(this);
		}
	}
}
