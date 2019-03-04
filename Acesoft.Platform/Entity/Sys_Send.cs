using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Platform.Entity
{
    [Table("sys_send")]
    public class Sys_Send : EntityBase
	{
		public long User_Id { get; set; }
		public long ParentId { get; set; }
        public string Sender { get; set; }
        public string Title { get; set; }
        public string Msg { get; set; }
        public MsgType Type { get; set; }
        public MsgStatus Stauts { get; set; }
        public string Rec_Ids { get; set; }
        public string Rec_Names { get; set; }
        public int Ref_Id { get; set; }
        public string Ref_Msg { get; set; }
        public int TyrTimes { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime DUpdate { get; set; }
    }
}
