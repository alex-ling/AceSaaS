using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.IoT.Config
{
    public class IotAccess
    {
        public bool Enabled { get; set; }
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public int CmdTimeout { get; set; }
        public bool AutoConnect { get; set; }
        public bool ReconnectFail { get; set; }
        public bool ReconnectClose { get; set; }
        public int ConnectInterval { get; set; }
        public string WebSocketUrl { get; set; }
        public string WebSocketUserName { get; set; }
        public string WebSocketPassword { get; set; }
    }
}
