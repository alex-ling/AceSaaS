using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class Panel : ContentWidgetBase
	{
		public static readonly ScriptEvent Extractor = new ScriptEvent("extractor", "data");

		public static readonly ScriptEvent OnLoad = new ScriptEvent("onLoad", "");

		public static readonly ScriptEvent OnBeforeOpen = new ScriptEvent("onBeforeOpen", "");

		public static readonly ScriptEvent OnOpen = new ScriptEvent("onOpen", "");

		public static readonly ScriptEvent OnBeforeClose = new ScriptEvent("onBeforeClose", "");

		public static readonly ScriptEvent OnClose = new ScriptEvent("onClose", "");

		public static readonly ScriptEvent OnBeforeDestroy = new ScriptEvent("onBeforeDestroy", "");

		public static readonly ScriptEvent OnDetroy = new ScriptEvent("onDetroy", "");

		public static readonly ScriptEvent OnBeforeCollapse = new ScriptEvent("onBeforeCollapse", "");

		public static readonly ScriptEvent OnCollapse = new ScriptEvent("onCollapse", "");

		public static readonly ScriptEvent OnBeforeExpand = new ScriptEvent("onBeforeExpand", "");

		public static readonly ScriptEvent OnExpand = new ScriptEvent("onExpand", "");

		public static readonly ScriptEvent OnResize = new ScriptEvent("onResize", "width,height");

		public static readonly ScriptEvent OnMove = new ScriptEvent("onMove", "left,top");

		public static readonly ScriptEvent OnMaximize = new ScriptEvent("onMaximize", "");

		public static readonly ScriptEvent OnRestore = new ScriptEvent("onRestore", "");

		public static readonly ScriptEvent OnMinimize = new ScriptEvent("onMinimize", "");

		public string Title
		{
			get;
			set;
		}

		public string IconCls
		{
			get;
			set;
		}

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

		public int? Left
		{
			get;
			set;
		}

		public int? Top
		{
			get;
			set;
		}

		public string Cls
		{
			get;
			set;
		}

		public string HeaderCls
		{
			get;
			set;
		}

		public string BodyCls
		{
			get;
			set;
		}

		public dynamic Style
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

		public bool? DoSize
		{
			get;
			set;
		}

		public bool? NoHeader
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}

		public Align? HAlign
		{
			get;
			set;
		}

		public Direction? TitleDirection
		{
			get;
			set;
		}

		public bool? Collapsible
		{
			get;
			set;
		}

		public bool? Minimizable
		{
			get;
			set;
		}

		public bool? Maximizable
		{
			get;
			set;
		}

		public bool? Closable
		{
			get;
			set;
		}

		public List<LinkButton> Tools
		{
			get;
			set;
		}

		public string Header
		{
			get;
			set;
		}

		public string Footer
		{
			get;
			set;
		}

		public Animation? OpenAnimation
		{
			get;
			set;
		}

		public int? OpenDuration
		{
			get;
			set;
		}

		public Animation? CloseAnimation
		{
			get;
			set;
		}

		public int? CloseDuration
		{
			get;
			set;
		}

		public bool? Collapsed
		{
			get;
			set;
		}

		public bool? Minimized
		{
			get;
			set;
		}

		public bool? Maximized
		{
			get;
			set;
		}

		public bool? Closed
		{
			get;
			set;
		}

		public string Href
		{
			get;
			set;
		}

		public bool? Cache
		{
			get;
			set;
		}

		public Panel(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "panel";
			Tools = new List<LinkButton>();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new PanelHtmlBuilder<Panel>(this);
		}
	}
}
