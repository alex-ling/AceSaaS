using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Acesoft.Util
{
    public static class NetHelper
    {
        public static IPAddress GetDomainIp(string domain)
        {
            var host = Dns.GetHostEntry(domain);
            if (host.AddressList.Any())
            {
                return host.AddressList.First();
            }
            return null;
        }
    }
}
