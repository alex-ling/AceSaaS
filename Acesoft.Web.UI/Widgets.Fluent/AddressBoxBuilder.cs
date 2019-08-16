using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI.Widgets.Fluent
{
    public class AddressBoxBuilder : ComboBuilder<AddressBox, AddressBoxBuilder>
    {
        public AddressBoxBuilder(AddressBox component)
            : base(component)
        { }

        public virtual AddressBoxBuilder Level(int level)
        {
            Component.Level = level;
            return this;
        }

        public virtual AddressBoxBuilder InitClick(bool initClick = true)
        {
            Component.IsInitClick = initClick;
            return this;
        }

        public AddressBoxBuilder Events(Action<AddressBoxEventBuilder> clientEventsAction)
        {
            clientEventsAction(new AddressBoxEventBuilder(base.Component.Events));
            return this;
        }
    }

    public class AddressBoxEventBuilder : ComboEventBuilder
    {
        public AddressBoxEventBuilder(IDictionary<string, object> events)
            : base(events)
        { }

        public AddressBoxEventBuilder OnSelect(string handler)
        {
            Handler(SelectBox.OnSelect.EventName, handler);
            return this;
        }
    }
}