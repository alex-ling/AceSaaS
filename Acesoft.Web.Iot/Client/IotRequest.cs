using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SuperSocket.ClientEngine;

namespace Acesoft.Web.IoT.Client
{
    public class IotRequest : IotResponse
    {
        public AutoResetEvent Wait { get; private set; }
        public IotResponse Response { get; set; }

        public Task Send(EasyClient client)
        {
            Wait = new AutoResetEvent(false);

            return Task.Run(() =>
            {
                client.Send(BuildBytes());
            });
        }
    }
}
