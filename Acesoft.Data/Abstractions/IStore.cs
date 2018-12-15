using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data
{
    public interface IStore : IDisposable
    {
        IConfiguration Configuration { get; set; }

        ISession OpenSession();


    }
}
