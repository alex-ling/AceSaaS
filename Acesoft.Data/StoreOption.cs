using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public class StoreOption : IStoreOption
    {
        public StoreOption()
        {
            IsolationLevel = IsolationLevel.ReadCommitted;
            AutoCreateSchema = true;
            SessionPoolSize = 16;
            QueryGatingEnabled = true;
        }

        public string Name { get; set; }
        public IsolationLevel IsolationLevel { get; set; }
        public IConnectionFactory ConnectionFactory { get; set; }
        public bool AutoCreateSchema { get; set; }
        public int SessionPoolSize { get; set; }
        public bool QueryGatingEnabled { get; set; }
        public string[] SqlMaps { get; set; }
    }
}
