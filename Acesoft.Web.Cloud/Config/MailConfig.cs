using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Cloud.Config
{
    public class MailConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string Sender { get; set; }
        public string From { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
    }
}
