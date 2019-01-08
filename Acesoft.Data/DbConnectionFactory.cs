using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Acesoft.Data
{
    public class DbConnectionFactory<TDbConnection> : IConnectionFactory
        where TDbConnection : DbConnection, new()
    {
        private readonly bool _shareConnection;
        private TDbConnection _sharedConnection;
        private readonly string _connectionString;
        private bool _disposing;

        public Type DbConnectionType => typeof(TDbConnection);

        public DbConnectionFactory(string connectionString, bool shareConnection = false)
        {
            _shareConnection = shareConnection;
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            if (_shareConnection)
            {
                if (_sharedConnection == null)
                {
                    lock (this)
                    {
                        if (_sharedConnection == null)
                        {
                            _sharedConnection = new TDbConnection();
                            _sharedConnection.ConnectionString = _connectionString;
                        }
                    }
                }

                return _sharedConnection;
            }

            var connection = new TDbConnection();
            connection.ConnectionString = _connectionString;

            return connection;
        }

        public void CloseConnection(IDbConnection connection)
        {
            if (_shareConnection)
            {
                // If the connection is shared, we don't close it
                return;
            }

            if (connection != null)
            {
                connection.Close();
            }
        }

        public void Dispose()
        {
            if (_disposing)
            {
                return;
            }

            _disposing = true;

            if (_shareConnection)
            {
                if (_sharedConnection != null)
                {
                    _sharedConnection.Dispose();
                }
            }
        }
    }
}
