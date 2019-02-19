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

        public static ISession Session => App.Context.RequestServices.GetRequiredService<ISession>();

        public static void UseDataContext(this IServiceProvider service)
        {
            IdWorker = service.GetService<IIdWorker>();
        }
    }
}
