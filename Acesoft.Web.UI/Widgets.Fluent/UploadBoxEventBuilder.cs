using Acesoft.Web.UI.Ajax;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Widgets.Fluent
{
	public class UploadBoxEventBuilder : EventBuilder
	{
		public UploadBoxEventBuilder(IDictionary<string, object> events)
			: base(events)
		{
		}

		public UploadBoxEventBuilder OnBeforeUpload(string handler)
		{
			Handler(UploadBox.OnBeforeUpload.EventName, handler);
			return this;
		}

		public UploadBoxEventBuilder OnUploaded(string handler)
		{
			Handler(UploadBox.OnUploaded.EventName, handler);
			return this;
		}
	}
}
