using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace Acesoft.Config
{
    public interface IConfig
    {
        string ConfigFile { get; set; }
        IConfigurationRoot Configuration { get; set; }
    }
}
