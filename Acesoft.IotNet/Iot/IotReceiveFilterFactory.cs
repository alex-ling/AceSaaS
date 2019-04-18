using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Acesoft.IotNet.Iot
{
    public class IotReceiveFilterFactory : IReceiveFilterFactory<IotRequest>
    {
        public IReceiveFilter<IotRequest> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            var opts = appServer.Config.Options;
            var filter = new IotReceiveFilter(opts["header"], Convert.ToInt32(opts["cryptoKey"], 16));
            appSession.Items["filter"] = filter;

            return filter;
        }
    }
}
