using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac
{
    public interface IUAService : IService<Rbac_UA>
    {
        int DeleteByUser(long userId);
        int DeleteByRole(long roleId);
        void Save(long userId, string roleIds);
    }
}
