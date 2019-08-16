using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.WeChat.Entity
{
    [Table("wx_activity_view")]
    public class Wx_Activity : EntityBase
    {
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
        public string Name { get; set; }
        public DateTime DStart { get; set; }
        public DateTime DEnd { get; set; }
        public string Poster { get; set; }
        public string Remark { get; set; }
        public string QrCode { get; set; }
        public string Url { get; set; }
    }
}