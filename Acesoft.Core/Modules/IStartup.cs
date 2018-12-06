using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Core.Modules
{
    /// <summary>
    /// 模块启动接口，用于初始化一个HTTP中间件/管道
    /// </summary>
    public interface IStartup
    {
        /// <summary>
        /// 设定一个启动顺序，默认为0
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 由框架调用，用于应用配置、注入等
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// 由框架调用，用于应用启动、配置、加载中间件等
        /// </summary>
        /// <param name="builder"><seealso cref="IApplicationBuilder"/></param>
        /// <param name="routes"><seealso cref="IRouteBuilder"/></param>
        /// <param name="serviceProvider"><seealso cref="IServiceProvider"/></param>
        void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider);
    }
}