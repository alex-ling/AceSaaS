using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Rbac.Entity;

namespace Acesoft.Rbac
{
    public interface IScaleService : IService<Rbac_Scale>
    {
        Rbac_Scale GetByRef(string refId);
        int Delete(long id);
        int DeleteByUser(long userId);
    }
}
