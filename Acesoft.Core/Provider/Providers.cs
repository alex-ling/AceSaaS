using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Core
{
    public class ProviderContainer : IProviders
    {
        readonly ConcurrentDictionary<string, object> _providers;

        public ProviderContainer()
        {
            _providers = new ConcurrentDictionary<string, object>();
        }

        public T GetProvider<T>(string key, Func<string, T> newInstance)
        {
            return (T)_providers.GetOrAdd(key, _ => newInstance(_));
        }
    }
}
