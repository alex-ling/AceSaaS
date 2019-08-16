using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.WeChat.Entity
{
    [Table("wx_votelog")]
    public class Wx_VoteLog : EntityBase
    {
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
        public long Vote_Id { get; set; }
        public long VoteItem_Id { get; set; }
        public string OpenId { get; set; }
        public DateTime DDay { get; set; }
        public int Count { get; set; }
        public string Content { get; set; }
    }
}