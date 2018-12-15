using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public class TenantContext<T> : IDisposable
    {
        private bool disposed;

        public string Id { get; } = Guid.NewGuid().ToString();
        public T Tenant { get; }
        public IDictionary<string, object> Properties { get; }

        public TenantContext(T tenant)
        {
            Check.Require(tenant != null, $"{nameof(Tenant)}不能为Null");

            Tenant = tenant;
            Properties = new Dictionary<string, object>();
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
