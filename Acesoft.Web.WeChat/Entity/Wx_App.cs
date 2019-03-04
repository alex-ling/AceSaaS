using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.WeChat.Entity
{
	[Table("wx_app")]
	public class Wx_App : EntityBase
	{
		public string Name { get; set; }
		public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string Token { get; set; }
        public string AesKey { get; set; }
        public WxType Type { get; set; }
        public string Secret { get; set; }
        public string Remark { get; set; }
    }
}
