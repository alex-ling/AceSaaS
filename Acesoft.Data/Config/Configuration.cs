using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data.Config
{
    public class Configuration : IConfiguration
    {
        public Configuration()
        {
            IsolationLevel = IsolationLevel.ReadCommitted;
            TablePrefix = "";
            SessionPoolSize = 16;
            QueryGatingEnabled = true;
        }

        public IsolationLevel IsolationLevel { get; set; }
        public IConnectionFactory ConnectionFactory { get; set; }
        public string TablePrefix { get; set; }
        public int SessionPoolSize { get; set; }
        public bool QueryGatingEnabled { get; set; }
    }
}
