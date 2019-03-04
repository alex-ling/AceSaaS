using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Data
{
    public abstract class ServiceBase
    {
        public ISession Session => App.Context.RequestServices.GetService<ISession>();
    }
}
