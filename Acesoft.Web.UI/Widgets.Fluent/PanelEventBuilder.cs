using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class PanelEventBuilder : EventBuilder
	{
		public PanelEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public PanelEventBuilder OnLoad(string handler)
		{
			Handler(Panel.OnLoad.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnBeforeOpen(string handler)
		{
			Handler(Panel.OnBeforeOpen.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnOpen(string handler)
		{
			Handler(Panel.OnOpen.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnBeforeClose(string handler)
		{
			Handler(Panel.OnBeforeClose.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnClose(string handler)
		{
			Handler(Panel.OnClose.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnBeforeDestroy(string handler)
		{
			Handler(Panel.OnBeforeDestroy.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnDetroy(string handler)
		{
			Handler(Panel.OnDetroy.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnBeforeCollapse(string handler)
		{
			Handler(Panel.OnBeforeCollapse.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnCollapse(string handler)
		{
			Handler(Panel.OnCollapse.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnBeforeExpand(string handler)
		{
			Handler(Panel.OnBeforeExpand.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnExpand(string handler)
		{
			Handler(Panel.OnExpand.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnResize(string handler)
		{
			Handler(Panel.OnResize.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnMove(string handler)
		{
			Handler(Panel.OnMove.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnMaximize(string handler)
		{
			Handler(Panel.OnMaximize.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnRestore(string handler)
		{
			Handler(Panel.OnRestore.EventName, handler);
			return this;
		}

		public PanelEventBuilder OnMinimize(string handler)
		{
			Handler(Panel.OnMinimize.EventName, handler);
			return this;
		}
	}
}
