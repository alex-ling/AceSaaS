using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Web.UI.Script;
using Acesoft.Web.UI.Charts;

namespace Acesoft.Web.UI
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            Serizlizer.SerializerSettings.Converters.Add(new OptionConverter());
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            
        }
    }    
}
