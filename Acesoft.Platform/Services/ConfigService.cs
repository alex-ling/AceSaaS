using System;
using System.Collections.Generic;
using System.Linq;

using Acesoft.Data;
using Acesoft.Platform.Models;

namespace Acesoft.Platform.Services
{
	public class ConfigService : ServiceBase, IConfigService
	{
        public IDictionary<string, string> GetItems(long cfgId)
        {
            return Session.Query<ConfigItem>(
                new RequestContext("sys", "get_sys_cfg")
                .SetParam(new
                {
                    cfgId
                })
            ).ToDictionary(c => c.Key, c => c.Value);
        }
	}
}
