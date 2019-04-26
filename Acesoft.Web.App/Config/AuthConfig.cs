using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.App.Config
{
    public class AuthConfig
    {
        public string AuthCookie { get; set; }
        public int ExpiredDays { get; set; }
        public string LoginUrl { get; set; }
        public string LogoutUrl { get; set; }
        public string DenyUrl { get; set; }

        public bool Enabled { get; set; }
        public string CertPath { get; set; }
        public string CertPassword { get; set; }
        public string AuthUrl { get; set; }
        public bool UseHttps { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientScope { get; set; }
    }
}