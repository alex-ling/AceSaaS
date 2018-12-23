using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Acesoft.Core;

namespace Acesoft.Data
{
    public static class DataContext
    {
        public static IIdWorker IdWorker { get; private set; }

        public static ISession Session => null;

        public static void UseIdWorker(this IServiceProvider service)
        {
            IdWorker = service.GetService<IIdWorker>();
        }
    }
}
