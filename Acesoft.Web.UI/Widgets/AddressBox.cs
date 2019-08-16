using System;
using System.Collections.Generic;

using Acesoft.Web.UI.Ajax;
using Acesoft.Web.UI.Html;
using Acesoft.Web.UI.Widgets.Html;

namespace Acesoft.Web.UI.Widgets
{
    public class AddressBox : Combo
    {
        public int? Level { get; set; }
        public bool? IsInitClick { get; set; } = false;

        public static readonly ScriptEvent OnSelect = new ScriptEvent("onSelect", "id,text");

        public AddressBox(WidgetFactory ace)  : base(ace)
        {
            this.Widget = "addressbox";
        }

        protected override IHtmlBuilder GetHtmlBuilder()
        {
            return new AddressBoxHtmlBuilder(this);
        }
    }
}