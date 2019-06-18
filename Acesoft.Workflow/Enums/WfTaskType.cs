using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Workflow
{
    public enum WfTaskType : int
    {
        Default = 0,
        Start = 1,
        Stop = 2,
        Multiple = 3,
        Looping = 4,
        SubProcess = 5
    }
}
