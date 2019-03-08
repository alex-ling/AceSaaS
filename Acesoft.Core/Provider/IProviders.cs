using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Core
{
    public interface IProviders
    {
        T GetProvider<T>(string key, Func<string, T> newInstance);
    }
}
