using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public interface ITenant<out T>
    {
        T Value { get; }
    }
}
