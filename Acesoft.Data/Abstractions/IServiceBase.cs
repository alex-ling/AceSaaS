using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data
{
    public interface IServiceBase
    {
        ISession Session { get; }
    }

    public interface IStoreServiceBase : IServiceBase
    {
        bool NeedCloseSession { get; }
    }
}
