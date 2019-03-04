using System;

using Acesoft.Data;
using Dapper.Contrib.Extensions;

namespace Acesoft.Platform.Entity
{ 
    [Table("sys_app")]
    public class Sys_App : EntityBase
	{
		public string Name { get; set; }
        public string Remark { get; set; }
        public string AdrVersion { get; set; }
        public DateTime AdrPubDate { get; set; }
        public string AdrPackage { get; set; }
        public string AdrChangeLog { get; set; }
        public bool AdrForce { get; set; }
        public string IosVersion { get; set; }
        public DateTime IosPubDate { get; set; }
        public string IosPackage { get; set; }
        public string IosChangeLog { get; set; }
        public bool IosForce { get; set; }
    }
}
