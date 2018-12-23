using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public interface ITenant
    {
        Tenant Value { get; }
    }
}
