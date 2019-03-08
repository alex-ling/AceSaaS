using System;
using System.Collections.Generic;

using Acesoft.Data;
using Acesoft.Platform.Models;

namespace Acesoft.Platform
{
    public interface IConfigService
    {
        Configs GetConfig(long configId);
    }
}
