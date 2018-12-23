using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data.Config
{
    public static class ConfigExtensions
    {
        public static IConfiguration SetIsolationLevel(this IConfiguration configuration, IsolationLevel isolationLevel)
        {
            configuration.IsolationLevel = isolationLevel;
            return configuration;
        }

        public static IConfiguration SetConnectionFactory(this IConfiguration configuration, IConnectionFactory connectionFactory)
        {
            configuration.ConnectionFactory = connectionFactory;
            return configuration;
        }

        public static IConfiguration SetTablePrefix(this IConfiguration configuration, string tablePrefix)
        {
            configuration.TablePrefix = tablePrefix;
            return configuration;
        }

        public static IConfiguration SetSessionPoolSize(this IConfiguration configuration, int size)
        {
            configuration.SessionPoolSize = size;
            return configuration;
        }

        public static IConfiguration DisableQueryGating(this IConfiguration configuration)
        {
            configuration.QueryGatingEnabled = false;
            return configuration;
        }
    }
}
