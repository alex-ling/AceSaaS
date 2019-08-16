using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Workflow.Services;
using Acesoft.Workflow.Runtime;

namespace Acesoft.Workflow
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // regist services
            services.AddSingleton<IRouteService, RouteService>();
            services.AddSingleton<ITaskService, TaskService>();
            services.AddSingleton<IProcessService, ProcessService>();
            services.AddSingleton<IInstanceService, InstanceService>();
            services.AddSingleton<IInstanceTaskService, InstanceTaskService>();

            // regist runtime
            services.AddSingleton<IRuntimeStartup, WfRuntimeStartup>();
            services.AddSingleton<IRuntimeFetch, WfRuntimeFetch>();
            services.AddSingleton<IRuntimeForward, WfRuntimeForward>();
            services.AddSingleton<IRuntimeBackward, WfRuntimeBackward>();
            services.AddSingleton<IRuntimeWithdraw, WfRuntimeWithdraw>();

            // regist workflowservice
            services.AddSingleton<IWorkflowService, WorkflowService>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {            
        }
    }
}
   