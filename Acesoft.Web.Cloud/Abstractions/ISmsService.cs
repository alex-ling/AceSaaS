using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Cloud
{
    public interface ISmsService
    {
        void Send(string phone, string signName, string templateCode, object param, string outId = null);
    }
}
