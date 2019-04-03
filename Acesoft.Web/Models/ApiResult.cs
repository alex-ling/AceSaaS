using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Models
{
    public class ApiResult
    {
        public int status { get; set; }
        public object value { get; set; }
        public string error { get; set; }
    }
}
