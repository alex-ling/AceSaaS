using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web
{
    public interface IApplicationStateProvider
    {
        string Name { get; }
        Func<IApplicationContext, T> Get<T>();
    }
}
