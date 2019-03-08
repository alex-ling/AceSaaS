using System;
using System.Collections.Generic;
using System.Linq;

using Acesoft.Data;
using Acesoft.Platform.Models;

namespace Acesoft.Platform.Services
{
	public class ConfigService : ServiceBase, IConfigService
	{
        public Configs GetConfig(long cfgId)
        {
            return new Configs(
                Session.Query<ConfigItem>(
                    new RequestContext("sys", "get_sys_cfg")
                    .SetParam(new
                    {
                        cfgId
                    })
                ).ToDictionary(c => c.Key)
            );
        }
	}
}
