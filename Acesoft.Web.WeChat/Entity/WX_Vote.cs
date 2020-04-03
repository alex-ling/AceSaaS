using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.WeChat.Entity
{
    [Table("wx_vote")]
    public class Wx_Vote : EntityBase
    {
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Logo { get; set; }
        public DateTime DStart { get; set; }
        public DateTime DEnd { get; set; }
        public WxVote VoteType { get; set; }
        public int VoteCount { get; set; }
    }
}