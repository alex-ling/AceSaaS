using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Acesoft.Rbac;
using System.Linq;
using Acesoft.Web.Multitenancy;

namespace Acesoft.Web
{
    public class ApplicationContext : IApplicationContext
    {
        private IHttpContextAccessor httpContextAccessor;
        private readonly ConcurrentDictionary<string, Func<object>> stateResolvers;
        private IEnumerable<IApplicationStateProvider> stateProviders;

        public IHostingEnvironment HostingEnvironment { get; }
        public HttpContext HttpContext => httpContextAccessor.HttpContext;

        public TenantContext TenantContext => HttpContext.GetTenantContext();
        public Tenant Tenant => TenantContext.Tenant;

        public bool IsAuthenticated => HttpContext.User.Identity.IsAuthenticated;
        public IUser CurrentUser => Get<IUser>(nameof(CurrentUser));
        public IUser CurrentCustomer => Get<IUser>(nameof(CurrentCustomer));

        public Acesoft.Data.ISession DbSession => HttpContext.RequestServices.GetRequiredService<Acesoft.Data.ISession>();

        public ApplicationContext(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.HostingEnvironment = hostingEnvironment;

            this.stateResolvers = new ConcurrentDictionary<string, Func<object>>();
        }
        
        public T As<T>() where T : class, IApplicationContext
        {
            return this as T;
        }

        public T Get<T>(string name)
        {
            var state = stateResolvers.GetOrAdd(name, key => FindResolverForState<T>(key));
            if (state != null)
            {
                return (T)state();
            }
            return default(T);
        }

        public void Set(string name, object value)
        {
            if (stateResolvers.ContainsKey(name))
            {
                stateResolvers[name] = () => value;
            }
            else
            {
                stateResolvers.TryAdd(name, () => value);
            }
        }

        private Func<object> FindResolverForState<T>(string name)
        {
            if (stateProviders == null)
            {
                stateProviders = HttpContext.RequestServices.GetServices<IApplicationStateProvider>();
            }

            var resolver = stateProviders.FirstOrDefault(m => m.Name == name).Get<T>();
            if (resolver == null)
            {
                return () => default(T);
            }

            return () => resolver(this);
        }
    }
}
