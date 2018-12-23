using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Web.Modules
{
    public interface IModulesHost
    {
        IDictionary<string, ModuleWarpper> Modules { get; }

        void Initialize();
    }
}
