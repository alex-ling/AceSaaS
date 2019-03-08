using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;

namespace Acesoft
{
    public interface IStateProvider
    {
        string Name { get; }
        Func<HttpContext, object> Get();
    }
}
