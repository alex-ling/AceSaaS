using System.Collections.Generic;

namespace Acesoft.Web.UI.Ajax
{
	public struct ScriptEvent
	{
		public string EventName;

		public string EventParams;

		public ScriptEvent(string eventName, string eventParams)
		{
			EventName = eventName;
			EventParams = eventParams;
		}

		public static void Regist(IDictionary<string, object> events, string @event, string handler)
		{
			events[@event] = new ScriptHandler
			{
				Handler = handler
			};
		}

		public static void Regist(IDictionary<string, object> events, ScriptEvent @event, string handler)
		{
			events[@event.EventName] = new ScriptHandler
			{
				Handler = handler
			};
		}
	}
}
