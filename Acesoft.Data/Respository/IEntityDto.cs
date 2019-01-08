using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public interface IEntityDto
    {
        void Load(DataRow row);
    }
}
