using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Platform.Office
{
    public interface IOfficeReport
    {
        byte[] Export();
    }
}
