using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperSocket.ClientEngine;
using Acesoft.Config;
using Acesoft.Web.IoT.Models;
using Acesoft.Web.IoT.Config;

namespace Acesoft.Web.IoT.Client
{
    public class IotClient : IIotClient
    {
        private readonly ILogger<IotClient> logger;
        private readonly IotAccess access;

        public EasyClient Client { get; }

        public IotClient(ILogger<IotClient> logger, IOptions<IotConfig> iotOptions)
        {
            this.logger = logger;
            this.access = iotOptions.Value.Servers["iot"];

            this.Client = new EasyClient();
            this.Client.Initialize(new IotReceiveFilter(), this.OnReceived);
            this.Client.Closed += (sender, e) =>
            {
                if (access.ReconnectClose)
                {
                    var t = Open();
                }
            };

            if (access.AutoConnect)
            {
                var t = Open();
            }
        }

        #region open&close
        public async Task Open()
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(access.ServerIp), access.ServerPort);
            if (await Client.ConnectAsync(endpoint))
            {
                logger.LogDebug("IotClient has connected to server!");
            }
            else
            {
                logger.LogWarning("IotClient has't connected to server!");

                if (access.ReconnectFail)
                {
                    await Task.Delay(access.ConnectInterval);
                    var t = Open();
                }
            }
        }

        public void Close()
        {
            this.Client.Close();
            logger.LogInformation("IotClient has closed!");
        }
        #endregion

        #region send&receive
        public Task<string> Send(IotRequest request)
        {
            throw new NotImplementedException();
        }

        private void OnReceived(IotResponse response)
        {
        }
        #endregion
    }
}
