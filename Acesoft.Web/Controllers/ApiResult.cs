using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Controllers
{
    public class ApiResult
    {
        public int http_status;
        public object value;
        public string error_code;
        public string error_msg;
    }
}
