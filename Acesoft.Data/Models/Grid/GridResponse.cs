using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public class GridResponse
    {
        public GridRequest Request { get; set; }
        public string SqlMapName { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
        public DataTable Data { get; set; }
    }

    public class GridResponse<T>
    {
        public GridRequest Request { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
