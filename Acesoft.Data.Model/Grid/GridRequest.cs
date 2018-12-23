using System;
using System.Collections.Generic;
using System.Text;

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
}
