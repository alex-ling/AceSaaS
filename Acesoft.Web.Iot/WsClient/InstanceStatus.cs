using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.IoT.WsClient
{
    public class InstanceStatus
    {
        public string Name { get; set; }
        public int MaxConnectionNumber { get; set; }
        public string Listeners { get; set; }
        public bool IsRunning { get; set; }
        public DateTime? StartedTime { get; set; }
        public int TotalConnections { get; set; }
        public int RequestHandlingSpeed { get; set; }
        public int TotalHandledRequests { get; set; }
        public int AvialableSendingQueueItems { get; set; }
        public int TotalSendingQueueItems { get; set; }
        public DateTime CollectedTime { get; set; }
    }
}
