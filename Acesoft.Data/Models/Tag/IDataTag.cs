using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public interface IDataTag
    {
        object DataRow { get; }
        string Expression { get; }
        int RowIndex { get; }

        string Output();
    }
}
