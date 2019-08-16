using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.ClientEngine;
using Acesoft.Web.IoT.Client;
using Acesoft.Web.Multitenancy;

namespace Acesoft.Web.IoT
{
    public interface IIotClient
    {
        EasyClient Client { get; }

        Task Open();
        void Close();

        Task<IotContext> Send(IotRequest request);
    }
}
