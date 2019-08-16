using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Workflow
{
    public enum WfAuditState : int
    {
        UnSend = 0,
        Pass = 1,
        Back = 2,
        Pending = 3,
        UnPass = 4,
        Sended = 5
    }
}
