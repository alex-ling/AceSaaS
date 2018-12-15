using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public class TreeRequest
    {
        public int? Id { get; set; }
        public string RootId { get; set; }
        public string RootName { get; set; }
        public string RootIcon { get; set; }
    }

    public class TreeResponse
    {
        public TreeRequest Request { get; set; }
        public DataTable Data { get; set; }
    }
}
