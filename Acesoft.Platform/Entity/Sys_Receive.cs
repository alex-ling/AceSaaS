using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Platform.Entity
{
    [Table("sys_receive")]
    public class Sys_Receive : EntityBase
	{
		public long Send_Id { get; set; }
		public long User_Id { get; set; }
        public MsgStatus Status { get; set; }
        public string RecAddr { get; set; }
        public int TryTimes { get; set; }
        public DateTime DCreate { get; set; }
        public DateTime DUpdate { get; set; }
    }
}
