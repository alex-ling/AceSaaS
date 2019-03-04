using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets
{
	public class Pagination : WidgetBase
	{
		public static readonly ScriptEvent OnSelectPage = new ScriptEvent("onSelectPage", "page,size");

		public static readonly ScriptEvent OnBeforeRefresh = new ScriptEvent("onBeforeRefresh", "page,size");

		public static readonly ScriptEvent OnRefresh = new ScriptEvent("onRefresh", "page,size");

		public static readonly ScriptEvent OnChangePageSize = new ScriptEvent("onChangePageSize", "size");

		public int? Total
		{
			get;
			set;
		}

		public int? PageSize
		{
			get;
			set;
		}

		public int? PageNumber
		{
			get;
			set;
		}

		public List<int> PageList
		{
			get;
			set;
		}

		public bool? Loading
		{
			get;
			set;
		}

		public List<LinkButton> Buttons
		{
			get;
			set;
		}

		public List<PaginationItem> Layout
		{
			get;
			set;
		}

		public int? Links
		{
			get;
			set;
		}

		public bool? ShowPageList
		{
			get;
			set;
		}

		public bool? ShowRefresh
		{
			get;
			set;
		}

		public bool? ShowPageInfo
		{
			get;
			set;
		}

		public string BeforePageText
		{
			get;
			set;
		}

		public string AfterPageText
		{
			get;
			set;
		}

		public string DisplayMsg
		{
			get;
			set;
		}

		public Pagination(WidgetFactory ace)
			: base(ace)
		{
			base.Widget = "pagination";
			PageList = new List<int>();
			Buttons = new List<LinkButton>();
			Layout = new List<PaginationItem>();
		}

		protected override IHtmlBuilder GetHtmlBuilder()
		{
			return new PaginationHtmlBuilder(this);
		}
	}
}
