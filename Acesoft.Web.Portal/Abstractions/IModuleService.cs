using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Data;
using Acesoft.Web.Portal.Entity;
using Acesoft.Web.UI;
using Newtonsoft.Json.Linq;

namespace Acesoft.Web.Portal
{
    public interface IModuleService : IService<Port_Module>
    {
        IEnumerable<Port_Module> QueryByPage(Port_Page page);
        Port_Module QueryById(long id);

        Port_Module AddModule(Port_Page page, Port_Widget widget, string dockName);
        void SaveConfig(Port_Module module, IDictionary<string, object> data);
        void Update(long id, string dockName, long? prevModId);
        void Delete(long id);

        Task<string> LoadModule(WidgetFactory ace, Port_Module module, bool design);
    }
}