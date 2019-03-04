using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class EditorEventBuilder : EventBuilder
	{
		public EditorEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public EditorEventBuilder OnAfterCreate(string handler)
		{
			Handler(KindEditor.OnAfterCreate.EventName, handler);
			return this;
		}

		public EditorEventBuilder OnAfterChange(string handler)
		{
			Handler(KindEditor.OnAfterChange.EventName, handler);
			return this;
		}

		public EditorEventBuilder OnAfterTab(string handler)
		{
			Handler(KindEditor.OnAfterTab.EventName, handler);
			return this;
		}

		public EditorEventBuilder OnAfterFocus(string handler)
		{
			Handler(KindEditor.OnAfterFocus.EventName, handler);
			return this;
		}

		public EditorEventBuilder OnAfterBlur(string handler)
		{
			Handler(KindEditor.OnAfterBlur.EventName, handler);
			return this;
		}

		public EditorEventBuilder OnAfterUpload(string handler)
		{
			Handler(KindEditor.OnAfterUpload.EventName, handler);
			return this;
		}

		public EditorEventBuilder OnAfterSelectFile(string handler)
		{
			Handler(KindEditor.OnAfterSelectFile.EventName, handler);
			return this;
		}
	}
}
