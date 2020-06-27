# AceSaaS
An SaaS/Multi-Tenants/Multi-Modules AspNetCore Framework. Like OrchardCore, but it's lightweight.  
多租户/多模块轻量级AspNetCore框架

## Multi-Modules - 多模块
将整个系统划分为各个独立的模块（解藕），模块之间通过依赖实现关联。  
参考OrchardCore源码，定义IStartup模块全局接口:
```csharp
interface IStartup
{
    int Order { get; }
    void ConfigureServices(IServiceCollection services);
    void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services);
}
```    
每个模块定义入口类 [Startup] 继承自 IStartup，实现模块的DI注入、启用配置、应用中间件等。  
框架内实现了多个内部模块，如：Acesoft.Logger，Acesoft.Config，Acesoft.Cache，Acesoft.Web。  
其中 [Acesoft.Web] 是整个应用的启用入口，内部会自动加载内部模块（引用的dll文件）和外部模块（非引用模块）

## Multi-tenants/SaaS - 多租户
基于hostname和port进行区分，多个Tenants运行于同一AppDomain内，共享Mvc和Pages。  
每个Tenant适配不同的数据库，实现库级安全隔离。同时提供一个Tenants管理库，用于SaaS创建和管理。    
每个Tenant包含多个外部Modules，Modules可以相互依赖，依据Modules可以组合出不同的客户服务。  
Module也可以对应于Href中的segments。每个模块也可以单独定义数据库。  
Tenant通过注册中间件以启用，可以配置默认Tenant或unresolvedRedirect。  

下为多个Tenants的配置文件 tenants.config.json:  
```json
{
  "defaultTenant": "default",
  "unresolvedRedirect": "http://www.miniplat.com",
  "RedirectPermanent": false,
  "tenants": [
    {
      "name": "saas",
      "hostnames": [
        "localhost:5000",
        "localhost:5001"
      ],
      "modules": [
        "admin",
        "platform"
      ],
      "theme": "default"
    }
  ]
}
```
## Getting started - 快速开始  
创建一个AspNetCore应用，引用Acesoft.Web，然后配置Startup文件：
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMultitenancy();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Use SaaS middleware
    app.UseMultitenancy();
}
```
## NetCore3.1
现已更新为NetCore3.1，长期更新版本 [AcePlat](https://github.com/alex-ling/aceplat)
