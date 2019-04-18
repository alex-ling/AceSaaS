using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Web.IoT.WsClient;

namespace Acesoft.Web.IoT
{
    public interface IIotWsClient
    {
        event ServiceStatusHandler Status;
        event ServiceErrorHandler Error;

        ServiceStatus CurrentStatus { get; }

        void Connect();
        void Start(string serverName);
        void Stop(string serverName);
        void Restart(string serverName);
    }
}
