using Acesoft.Web.UI.Html;
using System.Linq;

namespace Acesoft.Web.UI.Widgets.Html
{
	public class PaginationHtmlBuilder : EasyUIHtmlBuilder<Pagination>
	{
		public PaginationHtmlBuilder(Pagination component)
			: base(component, "div")
		{
		}

		protected override void PreBuild()
		{
			base.PreBuild();
			if (base.Component.Total.HasValue)
			{
				base.Options["total"] = base.Component.Total;
			}
			if (base.Component.PageSize.HasValue)
			{
				base.Options["pageSize"] = base.Component.PageSize;
			}
			if (base.Component.PageNumber.HasValue)
			{
				base.Options["pageNumber"] = base.Component.PageNumber;
			}
			if (base.Component.PageList.Any())
			{
				base.Options["pageList"] = base.Component.PageList;
			}
			if (base.Component.Loading.HasValue)
			{
				base.Options["loading"] = base.Component.Loading;
			}
			if (base.Component.Buttons.Any())
			{
				base.Options["buttons"] = base.Component.Buttons;
			}
			if (base.Component.Layout.Any())
			{
				base.Options["layout"] = base.Component.Layout;
			}
			if (base.Component.Links.HasValue)
			{
				base.Options["links"] = base.Component.Links;
			}
			if (base.Component.ShowPageList.HasValue)
			{
				base.Options["showPageList"] = base.Component.ShowPageList;
			}
			if (base.Component.ShowRefresh.HasValue)
			{
				base.Options["showRefresh"] = base.Component.ShowRefresh;
			}
			if (base.Component.ShowPageInfo.HasValue)
			{
				base.Options["showPageInfo"] = base.Component.ShowPageInfo;
			}
			if (base.Component.BeforePageText.HasValue())
			{
				base.Options["beforePageText"] = base.Component.BeforePageText;
			}
			if (base.Component.AfterPageText.HasValue())
			{
				base.Options["afterPageText"] = base.Component.AfterPageText;
			}
			if (base.Component.DisplayMsg.HasValue())
			{
				base.Options["displayMsg"] = base.Component.DisplayMsg;
			}
		}
	}
}
