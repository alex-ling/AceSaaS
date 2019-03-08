using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data
{
    public interface IStoreSchema
    {
        void CreateSchema(ISession session);
        void DropSchema(ISession session);
        void InitializeData(ISession session);
    }
}
