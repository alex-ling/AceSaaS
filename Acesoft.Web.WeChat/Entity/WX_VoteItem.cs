using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Web.WeChat.Entity
{
    [Table("wx_voteitem_view")]
    public class Wx_VoteItem : EntityBase
    {
        public DateTime DCreate { get; set; }
        public DateTime? DUpdate { get; set; }
        public int Vote_Id { get; set; }
        public string Name { get; set; }
        public int Vote_Count { get; set; }
    }
}