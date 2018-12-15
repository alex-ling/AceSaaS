using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Acesoft.Data.SqlMapper;

namespace Acesoft.Data
{
    public class GridRequest
    {
        public int? Page { get; set; }
        public int? Rows { get; set; }
        public long? Id { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
    }

    public class GridResponse
    {
        public GridRequest Request { get; set; }
        public SqlMap Map { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
        public DataTable Data { get; set; }
    }

    public class GridResponse<T>
    {
        public GridRequest Request { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<T> Datas { get; set; }
    }
}