using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.WeChat.Entity
{
	[Table("wx_media")]
	public class Wx_Media : EntityBase
	{
		public DateTime DCreate { get; set; }
		public long App_Id { get; set; }
        public string Name { get; set; }
        public WxMedia Type { get; set; }
        public string Url { get; set; }
        public string MediaId { get; set; }
        public string WxUrl { get; set; }
    }
}
