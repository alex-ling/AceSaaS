using System.Collections.Generic;

namespace Acesoft.Web.UI.Ajax
{
	public class EventBuilder
	{
		protected IDictionary<string, object> Events
		{
			get;
			private set;
		}

		public EventBuilder(IDictionary<string, object> events)
		{
			Events = events;
		}

		protected void Handler(string name, string handler)
		{
			Events[name] = new ScriptHandler
			{
				Handler = handler
			};
		}

		public void On(Event @event, string handler)
		{
			Handler($"on{@event}", handler);
		}

		public void On(string @event, string handler)
		{
			Handler(@event, handler);
		}
	}
}
