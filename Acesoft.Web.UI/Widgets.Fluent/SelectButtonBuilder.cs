using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Html;

namespace Acesoft.Web.UI.Widgets.Fluent
{
    public class SelectButtonBuilder : LinkButtonBuilder<SelectButton, SelectButtonBuilder>
    {
        public SelectButtonBuilder(SelectButton component)
            : base(component)
        { }

        public virtual SelectButtonBuilder SelectUrl(string selectUrl)
        {
            Component.SelectUrl = selectUrl;
            return this;
        }

        public virtual SelectButtonBuilder ValueBox(string valueBox)
        {
            Component.ValueBox = valueBox;
            return this;
        }

        public virtual SelectButtonBuilder TextBox(string textBox)
        {
            Component.TextBox = textBox;
            return this;
        }

        public virtual SelectButtonBuilder DialogWidth(int dialogWidth)
        {
            Component.DialogWidth = dialogWidth;
            return this;
        }

        public virtual SelectButtonBuilder DialogHeight(int dialogHeight)
        {
            Component.DialogHeight = dialogHeight;
            return this;
        }

        public SelectButtonBuilder Events(Action<SelectButtonEventBuilder> clientEventsAction)
        {
            clientEventsAction(new SelectButtonEventBuilder(base.Component.Events));
            return this;
        }
    }

    public class SelectButtonEventBuilder : ButtonEventBuilder
    {
        public SelectButtonEventBuilder(IDictionary<string, object> events)
            : base(events)
        { }

        public SelectButtonEventBuilder OnSelect(string handler)
        {
            Handler(SelectButton.OnSelect.EventName, handler);
            return this;
        }
    }
}