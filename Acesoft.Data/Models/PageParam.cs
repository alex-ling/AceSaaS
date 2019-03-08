using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public struct PageParam
    {
        public string Table;
        public string Columns;
        public string Where;
        public string Groupby;
        public string Orderby;
        public int Page;
        public int PageSize;
    }
}
