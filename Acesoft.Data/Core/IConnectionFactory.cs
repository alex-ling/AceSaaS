using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public interface IConnectionFactory : IDisposable
    {
        IDbConnection CreateConnection();
        void CloseConnection(IDbConnection connection);
        Type DbConnectionType { get; }
    }
}