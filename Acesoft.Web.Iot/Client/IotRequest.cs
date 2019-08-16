using System;
using System.Collections.Generic;
using System.Text;

using SuperSocket.ProtoBase;
using Acesoft.Util;

namespace Acesoft.Web.IoT.Client
{
    public class IotRequest : IPackageInfo
    {
        public string Tenant { get; set; }
        public string Mac { get; set; }
        public string Cmd { get; set; }
        public string Body { get; set; }
        public DateTime DCreate { get; set; }

        public IotRequest()
        {
            DCreate = DateTime.Now;
        }

        public IotRequest(string data) : this()
        {
            var items = data.Split('#');
            Tenant = items[1];
            Mac = items[2];
            Cmd = items[3];
            Body = items[4];
        }

        public byte[] BuildBytes()
        {
            return EncodingHelper.ToBytes($"#{Tenant}#{Mac}#{Cmd}#{Body}#");
        }
    }
}
