using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac
{
    public interface IUOService : IService<Rbac_UO>
    {
        int DeleteByUser(long userId, string type);
        void Save(long userId, string type, string refIds);
    }
}
