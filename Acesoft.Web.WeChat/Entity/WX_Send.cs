using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.WeChat.Entity
{
	[Table("wx_send")]
	public class Wx_Send : EntityBase
	{
		public DateTime DCreate { get; set; }
		public long Media_Id { get; set; }
        public string MsgId { get; set; }
        public string ErrCode { get; set; }
        public string ErrMsg { get; set; }
    }
}
