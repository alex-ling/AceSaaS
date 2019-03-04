using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.WeChat.Entity
{
    [Table("wx_news")]
    public class Wx_News : EntityBase
    {
        public DateTime DCreate { get; set; }
        public long Media_Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Digest { get; set; }
        public string Content { get; set; }
        public string ThumbUrl { get; set; }
        public string ThumbMediaId { get; set; }
        public string ThumbWxUrl { get; set; }
        public string SourceUrl { get; set; }
        public bool ShowCover { get; set; }
        public int OrderNo { get; set; }
    }
}