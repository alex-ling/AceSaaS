using Acesoft.Data;

namespace Acesoft.Web.UI
{
	public class Paging
	{
		public int Total { get; set; }
		public int PageSize { get; set; } = 10;
		public int PageNumber { get; set; } = 1;
		public int PageCount { get; set; }

        public void Load(GridResponse res)
		{
			PageSize = res.Request.Rows.Value;
			PageNumber = res.Request.Page.Value;
			Total = res.Total;
			PageCount = res.PageCount;
		}

		public string ToJson()
		{
			return $"total:{Total},pagesize:{PageSize},pageNumber:{PageNumber},pageCount:{PageCount}";
		}
	}
}
