using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data
{
    public interface IIdGenerator
    {
        long GetNextId(ISession session);
    }
}
