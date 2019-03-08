using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac
{
    public interface IRoleService : IService<Rbac_Role>
    {
        int Delete(long id);
        void Delete(string roleIds);
    }
}
