using System;
using System.Collections.Generic;

namespace Acesoft.Web.Cloud
{
	public class CloudConfig
	{
		public IDictionary<string, CloudAccess> Accesses { get; set; }
        public IDictionary<string, MailConfig> MailConfigs { get; set; }
        public IDictionary<string, IDictionary<string, string>> Settings { get; set; }
	}
}
