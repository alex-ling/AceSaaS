using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Rbac
{
    public class Token
    {
        public string Access_token { get; set; }
        public DateTime Created { get; set; }
        public int Expires_in { get; set; }
        public string Refresh_token { get; set; }
        public string Token_type { get; set; }
    }
}
