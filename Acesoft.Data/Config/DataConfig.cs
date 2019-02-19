using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data.Config
{
    public class DataConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseType { get; set; }
        public string[] SqlMaps { get; set; }
    }
}
