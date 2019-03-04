using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.WeChat.Entity
{
	[Table("wx_menu")]
	public class Wx_Menu : EntityBase
	{
		public long App_Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public WxButton Type { get; set; }
        public string Value { get; set; }
        public string ExtraValue { get; set; }
        public int OrderNo { get; set; }
    }
}
