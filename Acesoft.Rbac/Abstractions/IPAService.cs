using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac
{
    public interface IPAService : IService<Rbac_PA>
    {
        IList<Rbac_PA> GetsByRoles(IList<long> roleIds);
        void Save(long roleId, string refIds);
        int DeleteByRole(long roleId);
        int DeleteByRefId(long refId);
    }
}
