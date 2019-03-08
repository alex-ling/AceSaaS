using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public class TreeResponse
    {
        public TreeRequest Request { get; set; }
        public DataTable Data { get; set; }
    }
}
