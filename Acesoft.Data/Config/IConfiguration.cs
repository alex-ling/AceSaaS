using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data.Config
{
    public interface IConfiguration
    {
        IsolationLevel IsolationLevel { get; set; }
        IConnectionFactory ConnectionFactory { get; set; }
        string TablePrefix { get; set; }
        int SessionPoolSize { get; set; }
        bool QueryGatingEnabled { get; set; }
    }
}
