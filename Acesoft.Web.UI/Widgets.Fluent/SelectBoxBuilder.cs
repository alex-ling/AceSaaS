using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI.Widgets.Fluent
{
    public class SelectBoxBuilder : ComboBuilder<SelectBox, SelectBoxBuilder>
    {
        public SelectBoxBuilder(SelectBox component)
            : base(component)
        { }

        public virtual SelectBoxBuilder SelectUrl(string selectUrl)
        {
            Component.SelectUrl = selectUrl;
            return this;
        }

        public SelectBoxBuilder Events(Action<SelectBoxEventBuilder> clientEventsAction)
        {
            clientEventsAction(new SelectBoxEventBuilder(base.Component.Events));
            return this;
        }
    }

    public class SelectBoxEventBuilder : ComboEventBuilder
    {
        public SelectBoxEventBuilder(IDictionary<string, object> events)
            : base(events)
        { }

        public SelectBoxEventBuilder OnSelect(string handler)
        {
            Handler(SelectBox.OnSelect.EventName, handler);
            return this;
        }
    }
}