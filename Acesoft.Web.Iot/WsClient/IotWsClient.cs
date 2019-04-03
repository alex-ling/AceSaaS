using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperSocket.ClientEngine;
using Acesoft.Web.IoT.Config;

namespace Acesoft.Web.IoT.WsClient
{
    public delegate void ServiceErrorHandler(string error);
    public delegate void ServiceStatusHandler(ServiceStatus status);

    public class IotWsClient : IIotWsClient
    {
        public event ServiceStatusHandler Status;
        public event ServiceErrorHandler Error;

        private readonly ILogger<IotWsClient> logger;
        private readonly IotAccess access;
        private WebSocket client;

        public IotWsClient(ILogger<IotWsClient> logger, IOptions<IotConfig> iotOptions)
        {
            this.logger = logger;
            this.access = iotOptions.Value.Servers["iot"];
        }

        public void Connect()
        {
            client = new WebSocket(access.WebSocketUrl);
            client.Error += Client_Error;
            client.Opened += Client_Opened;
            client.On<object>("UPDATE", OnServerUpdated);
            client.Open();
        }

        private void Client_Error(object sender, ErrorEventArgs e)
        {
            this.Error?.Invoke(e.Exception.GetMessage());
        }

        private void Client_Opened(object sender, EventArgs e)
        {
            var loginInfo = new
            {
                UserName = access.WebSocketUserName,
                Password = access.WebSocketPassword
            };
            client.Query<object>("LOGIN", loginInfo, OnLoggedIn);
        }

        private void OnLoggedIn(dynamic result)
        {
            if (result["Result"].ToObject<bool>())
            {
                this.OnServiceStatus(result.NodeStatus);
            }
            else
            {
                this.Error?.Invoke(result.Message);
            }
        }

        private void OnServerUpdated(dynamic result)
        {
            this.OnServiceStatus(result);
        }

        public void Start(string serverName)
        {
            client.Query<object>("START", serverName, OnActionCallback);
        }

        public void Stop(string serverName)
        {
            client.Query<object>("STOP", serverName, OnActionCallback);
        }

        public void Restart(string serverName)
        {
            client.Query<object>("RESTART", serverName, OnActionCallback);
        }

        private void OnActionCallback(dynamic result)
        {
            this.OnServiceStatus(result.NodeStatus);
        }

        private void OnServiceStatus(dynamic json)
        {
            dynamic val = json.BootstrapStatus.Values;
            var status = new ServiceStatus();
            status.AvailableWorkingThreads = val.AvailableWorkingThreads;
            status.AvailableCompletionPortThreads = val.AvailableCompletionPortThreads;
            status.MaxCompletionPortThreads = val.MaxCompletionPortThreads;
            status.MaxWorkingThreads = val.MaxWorkingThreads;
            status.TotalThreadCount = val.TotalThreadCount;
            status.CpuUsage = val.CpuUsage;
            status.MemoryUsage = val.MemoryUsage;

            foreach (dynamic i in json.InstancesStatus)
            {
                status.InstancesStatus.Add(new InstanceStatus
                {
                    Name = i.Name,
                    CollectedTime = i.CollectedTime,
                    MaxConnectionNumber = i.Values.MaxConnectionNumber,
                    Listeners = i.Values.Listeners,
                    IsRunning = i.Values.IsRunning,
                    StartedTime = i.Values.StartedTime,
                    TotalConnections = i.Values.TotalConnections,
                    RequestHandlingSpeed = i.Values.RequestHandlingSpeed,
                    TotalHandledRequests = i.Values.TotalHandledRequests,
                    AvialableSendingQueueItems = i.Values.AvialableSendingQueueItems,
                    TotalSendingQueueItems = i.Values.TotalSendingQueueItems
                });
            }

            this.Status?.Invoke(status);
        }
    }
}
