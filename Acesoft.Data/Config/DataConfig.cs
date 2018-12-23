using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data.Config
{
    public class DataConfig
    {
        public string DefaultStore { get; set; }
        public IDictionary<string, StoreConfig> Stores { get; set; }
    }

    public class StoreConfig
    {
        public string ConnectionString { get; set; }
        public Type ProviderType { get; set; }
        public string[] SqlMaps { get; set; }
    }
}
