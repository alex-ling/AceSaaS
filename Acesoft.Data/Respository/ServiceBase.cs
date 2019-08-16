using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Data
{
    public abstract class ServiceBase
    {
        public virtual ISession Session => App.Context?.RequestServices.GetService<ISession>();
    }

    public abstract class StoreServiceBase : ServiceBase
    {
        private readonly IStore store;
        private ISession session;

        public override ISession Session
        {
            get
            {
                session = base.Session;
                if (session == null)
                {
                    NeedCloseSession = true;
                    session = store.OpenSession();
                }
                return session;
            }
        }

        public bool NeedCloseSession { get; private set; }

        public StoreServiceBase(IStore store)
        {
            this.store = store;
        }
    }
}
