using System;
using Microsoft.Extensions.DependencyInjection;

using Acesoft.Platform;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
	public class IdBox : TextBox, IDataBind
    {
        public string Seed { get; set; }
        public string Prefix { get; set; }
        public string DateExp { get; set; }
        public int length { get; set; }
        public bool PreEnabled { get; set; }
        public bool AutoSave { get; set; }
        public bool Dynamic { get; set; }
        public int? Nary { get; set; }
        public bool NeedLoad { get; set; }

        public void DataBind()
        {
            if (NeedLoad)
            {
                var seedService = Ace.AppCtx.HttpContext.RequestServices.GetService<ISeedService>();
                var prefix = Prefix + (DateExp.HasValue() ? DateTime.Now.ToString(DateExp) : "");
                Value = seedService.Create(Seed, prefix, length, AutoSave, Nary);
            }
        }

        public IdBox(WidgetFactory ace) : base(ace)
        {
        }

        protected override IHtmlBuilder GetHtmlBuilder()
        {
            return new IdBoxHtmlBuilder(this);
        }
    }
}
