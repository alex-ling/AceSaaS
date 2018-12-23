using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Core
{
    public interface IIdWorker
    {
        long NextId();
        string NextStringId();
    }
}
