using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data.Config;

namespace Acesoft.Data
{
    public interface IStore : IDisposable
    {
        IConfiguration Configuration { get; set; }
        ISqlDialect Dialect { get; }

        ISession OpenSession();
    }
}
