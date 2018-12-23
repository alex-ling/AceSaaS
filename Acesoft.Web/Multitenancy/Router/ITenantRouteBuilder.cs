using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Routing;

namespace Acesoft.Web.Multitenancy
{
    public interface ITenantRouteBuilder
    {
        IRouteBuilder Build();

        void Configure(IRouteBuilder builder);
    }
}
