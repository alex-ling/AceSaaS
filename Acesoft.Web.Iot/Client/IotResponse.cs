using System;
using System.Collections.Generic;
using System.Text;

using SuperSocket.ProtoBase;
using Acesoft.Util;

namespace Acesoft.Web.IoT.Client
{
    public class IotResponse : IPackageInfo
    {
        public string Mac { get; set; }
        public string Cmd { get; set; }
        public string Body { get; set; }

        public IotResponse()
        {
        }

        public IotResponse(string hexData)
        {
            var items = hexData.Trim('#').Split('#');
            Mac = items[0];
            Cmd = items[1];
            Body = items[2];
        }

        public byte[] BuildBytes()
        {
            return EncodingHelper.ToBytes($"#{Mac}#{Cmd}#{Body}#");
        }
    }
}
