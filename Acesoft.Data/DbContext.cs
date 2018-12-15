using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.NetCore.Threading;

namespace Acesoft.Data
{
    public class DbContext
    {
        public const string DbContextKey = "acesoft_dbsession";
        public static bool AutoOpenSession { get; set; } = true;

        private static ISession innerSession
        {
            get => Local.Get<ISession>(DbContextKey); 
            set => Local.Set<ISession>(DbContextKey, value);
        }

        public static ISession Start()
        {
            if (innerSession != null)
            {
                throw new AceException("Do not duplicate db's session！");
            }

            var session = SessionFactory.Instance.OpenSession();
            innerSession = session;
            return session;
        }

        internal static void Dispose()
        {
            innerSession = null;
        }

        public static ISession Current
        {
            get
            {
                var session = innerSession;
                if (session == null && AutoOpenSession)
                {
                    session = Start();
                }
                return session;
            }
        }

        public static bool IsStarted
        {
            get { return innerSession != null; }
        }
    }
}
