using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public interface IStoreOption
    {
        string Name { get; set; }
        IsolationLevel IsolationLevel { get; set; }
        IConnectionFactory ConnectionFactory { get; set; }
        string TablePrefix { get; set; }
        int SessionPoolSize { get; set; }
        bool QueryGatingEnabled { get; set; }
        string[] SqlMaps { get; set; }
    }
}
