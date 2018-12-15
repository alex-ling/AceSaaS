using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Acesoft.Core
{
    /// <summary>
    /// Plugin启动接口，用于配置一个Plugin
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// 设定一个启动顺序，默认为0
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 由框架调用，用于Plugin配置、注入等
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// 由框架调用，用于Plugin启动、配置、加载中间件等
        /// </summary>
        /// <param name="app"><seealso cref="IApplicationBuilder"/></param>
        /// <param name="routes"><seealso cref="IRouteBuilder"/></param>
        /// <param name="services"><seealso cref="IServiceProvider"/></param>
        void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services);
    }
}