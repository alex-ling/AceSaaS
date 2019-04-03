using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.IoT.WsClient
{
    public class ServiceStatus
    {
        public int AvailableWorkingThreads { get; set; }
        public int AvailableCompletionPortThreads { get; set; }
        public int MaxCompletionPortThreads { get; set; }
        public int MaxWorkingThreads { get; set; }
        public int TotalThreadCount { get; set; }
        public float CpuUsage { get; set; }
        public float MemoryUsage { get; set; }

        public IList<InstanceStatus> InstancesStatus { get; } = new List<InstanceStatus>();
    }
}
