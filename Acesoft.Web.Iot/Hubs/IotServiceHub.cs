using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace Acesoft.Web.IoT.Hubs
{
    public class IotServiceHub : Hub
    {
        private readonly IIotWsClient iotWsClient;

        public IotServiceHub(IIotWsClient iotWsClient)
        {
            this.iotWsClient = iotWsClient;
        }

        public Task Send(object message)
        {
            return base.Clients.All.SendAsync("Send", message);
        }

        public override Task OnConnectedAsync()
        {
            return Clients.Caller.SendAsync("Send", iotWsClient.CurrentStatus);
        }
    }
}
