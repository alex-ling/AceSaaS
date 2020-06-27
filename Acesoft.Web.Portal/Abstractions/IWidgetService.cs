using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Web.Portal.Config;
using Acesoft.Web.Portal.Entity;

namespace Acesoft.Web.Portal
{
    public interface IWidgetService : IService<Port_Widget>
    {
        void Regist(string path, string folder, WidgetConfig config);
        Port_Widget GetByPath(string path);
    }
}