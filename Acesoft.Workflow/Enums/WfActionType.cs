using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Workflow
{
    public enum WfActionType : int
    {
        Begin = 0,
        Forward = 1,
        Backward = 2,
        Looping = 3,
        Fetch = 4,
        Withdraw = 5
    }
}
