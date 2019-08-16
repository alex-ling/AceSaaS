using System;
using System.Collections.Generic;

using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
    public class SelectButton : LinkButton
    {
        public string ValueBox { get; set; }
        public string TextBox { get; set; }
        public int? DialogWidth { get; set; }
        public int? DialogHeight { get; set; }
        public string SelectUrl { get; set; }

        public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "val,txt");

        public SelectButton(WidgetFactory ace)
            : base(ace)
        {
            ScriptEvent.Regist(Events, OnClick, "AX.btnShow");
        }

        protected override IHtmlBuilder GetHtmlBuilder()
        {
            return new SelectButtonHtmlBuilder(this);
        }
    }
}