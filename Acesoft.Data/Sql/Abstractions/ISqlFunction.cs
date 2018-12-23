using System;

namespace Acesoft.Data
{
    public interface ISqlFunction
    {
        string Render(string[] arguments);
    }
}
