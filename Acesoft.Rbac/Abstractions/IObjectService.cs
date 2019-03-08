using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac
{
    public interface IObjectService : IService<Rbac_Object>
    {
        Rbac_Object GetByUrl(string url);
        IList<Rbac_Object> Gets(IList<long> roleIds, ObjectType type, string user);
        int Delete(long id);
        int Delete(string ids);
    }
}
