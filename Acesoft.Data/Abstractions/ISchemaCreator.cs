using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data
{
    public interface ISchemaCreator
    {
        bool IsCreated(ISession session);
        void CreateSchema(ISession session);
    }
}
