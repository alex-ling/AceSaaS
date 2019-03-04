using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Database
{
    public interface ISchemaStore
    {
        void CreateSchema();
        void DropSchema();
    }
}
