using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data;
using Acesoft.Web.Portal.Config;
using Acesoft.Web.Portal.Entity;

namespace Acesoft.Web.Portal.Services
{
    public class WidgetService : Service<Port_Widget>, IWidgetService
    {
        public Port_Widget GetByPath(string path)
        {
            return Session.QueryFirst<Port_Widget>(
                new RequestContext("sys", "get_widget_by_path")
                .SetParam(new
                {
                    path
                })
            );
        }

        public Port_WidgetTemplate GetTemplateByPath(long widgetId, string path)
        {
            return Session.QueryFirst<Port_WidgetTemplate>(
                new RequestContext("sys", "get_widgettemplate_by_path")
                .SetParam(new
                {
                    widgetId,
                    path
                })
            );
        }

        public void Regist(string path, string folder, WidgetConfig config)
        {
            var widget = GetByPath(path);
            if (widget == null)
            {
                widget = new Port_Widget();
                widget.InitializeId();
                widget.DCreate = DateTime.Now;
                widget.Name = config.Name;
                widget.Folder = folder;
                widget.Path = path;
                widget.Version = config.Version;
                widget.Remark = config.Remark;
                Session.Insert(widget);
            }
            else
            {
                widget.DUpdate = DateTime.Now;
                widget.Name = config.Name;
                widget.Version = config.Version;
                widget.Remark = config.Remark;
                Session.Update(widget);
            }

            foreach (var tempConfig in config.Templates)
            {
                var temp = GetTemplateByPath(widget.Id, tempConfig.Path);
                if (temp == null)
                {
                    temp = new Port_WidgetTemplate();
                    temp.InitializeId();
                    temp.Widget_id = widget.Id;
                    temp.Name = tempConfig.Name;
                    temp.Path = tempConfig.Path;
                    temp.Remark = tempConfig.Remark;
                    Session.Insert(temp);
                }
                else
                {
                    temp.Name = tempConfig.Name;
                    temp.Remark = tempConfig.Remark;
                    Session.Update(temp);
                }
            }

            Session.FlushCache("sys.widget");
        }
    }
}
