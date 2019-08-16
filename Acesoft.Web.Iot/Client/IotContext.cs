using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SuperSocket.ClientEngine;

namespace Acesoft.Web.IoT.Client
{
    public class IotContext
    {
        public AutoResetEvent Wait { get; private set; }
        public IotRequest Request { get; set; }
        public IotRequest Response { get; set; }

        public IotContext(IotRequest request)
        {
            this.Request = request;
        }

        public Task Send(EasyClient client)
        {
            Wait = new AutoResetEvent(false);

            return Task.Run(() =>
            {
                client.Send(Request.BuildBytes());
            });
        }
    }
}
