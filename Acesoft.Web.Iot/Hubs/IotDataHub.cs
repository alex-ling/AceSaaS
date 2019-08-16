using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

namespace Acesoft.Web.IoT.Hubs
{
    public class IotDataHub : Hub
    {
        private readonly IIotService iotService;

        public IotDataHub(IIotService iotService)
        {
            this.iotService = iotService;
        }

        public Task Send(object message)
        {
            return base.Clients.All.SendAsync("Send", message);
        }

        public Task Group(string group, object message)
        {
            return base.Clients.Group(group).SendAsync("Send", message);
        }

        public async Task Join(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);

            var clientProxy = Clients.Group(group);
            await clientProxy.SendAsync("Send", iotService.GetData(group));
        }

        public Task Leave(string group)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
    }
}
