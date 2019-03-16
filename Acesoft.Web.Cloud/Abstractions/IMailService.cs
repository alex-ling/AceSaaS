using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Cloud
{
    public interface IMailService
    {
        void Send(string mailto, string subject, string body);
    }
}
