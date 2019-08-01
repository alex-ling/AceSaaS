using System;
using System.Collections.Generic;

using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
    public class SelectBox : Combo
    {
        public string SelectUrl { get; set; }
        public string Parent { get; set; }
        public string Param { get; set; }

        public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "id,text");

        public SelectBox(WidgetFactory ace)
            : base(ace)
        {
            this.Widget = "selectbox";
            this.PanelWidth = 750;
            this.PanelHeight = 450;
            ScriptEvent.Regist(Events, OnBeforeShowPanel, "AX.selShow");
        }

        protected override IHtmlBuilder GetHtmlBuilder()
        {
            return new SelectBoxHtmlBuilder(this);
        }
    }
}