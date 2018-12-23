using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Web.Multitenancy
{
    public class TenantContext : IDisposable
    {
        private bool disposed;

        public string Id { get; } = Guid.NewGuid().ToString();
        public Tenant Tenant { get; }
        public IServiceProvider ServiceProvider { get; }
        public IDictionary<string, object> Properties { get; }

        public TenantContext(Tenant tenant, IServiceProvider serviceProvider)
        {
            Check.Require(tenant != null, $"{nameof(Tenant)} cannot be null");

            Tenant = tenant;
            ServiceProvider = serviceProvider;
            Properties = new Dictionary<string, object>();
        }

        public IServiceScope EnterServiceScope()
        {
            return new ServiceScopeWrapper(ServiceProvider.CreateScope());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var prop in Properties)
                {
                    TryDisposeProperty(prop.Value as IDisposable);
                }

                TryDisposeProperty(Tenant as IDisposable);
            }

            disposed = true;
        }

        private void TryDisposeProperty(IDisposable obj)
        {
            if (obj == null)
            {
                return;
            }

            try
            {
                obj.Dispose();
            }
            catch (ObjectDisposedException)
            { }
        }
    }
}
