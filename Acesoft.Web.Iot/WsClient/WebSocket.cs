using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

using WebSocket4Net;

namespace Acesoft.Web.IoT.WsClient
{
    public class WebSocket : JsonWebSocket
    {
        public WebSocket(string uri) : base(uri)
        {
        }

        protected override string SerializeObject(object target)
        {
            return JsonConvert.SerializeObject(target);
        }

        protected override object DeserializeObject(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }
    }
}
