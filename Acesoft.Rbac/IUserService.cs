using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;

namespace Acesoft.Rbac
{
    public interface IUserService : IService<IUser>
    {
        IUser Get(string name);
    }
}
