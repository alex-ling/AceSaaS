using System.Collections.Generic;

namespace Acesoft.Web.UI.Ajax
{
	public class AjaxObject : JsonObject
	{
		public static readonly ScriptEvent DataFilter = new ScriptEvent("dataFilter", "data,type");

		public static readonly ScriptEvent OnBeforeSend = new ScriptEvent("beforeSend", "xhr");

		public static readonly ScriptEvent OnComplete = new ScriptEvent("complete", "xhr,status");

		public static readonly ScriptEvent OnError = new ScriptEvent("error", "xhr,status,error");

		public static readonly ScriptEvent OnSuccess = new ScriptEvent("success", "data,status,xhr");

		public string Accepts { get; set; }

		public bool? Async { get; set; }

		public bool? Cache { get; set; }

		public string Contents { get; set; }

		public string ContentType { get; set; }

		public string Context { get; set; }

		public string Converters { get; set; }

		public bool? CrossDomain { get; set; }

		public string Data { get; set; }

		public DataType? DataType { get; set; }

		public bool? Global { get; set; }

		public string Headers { get; set; }

		public bool? IfModified { get; set; }

		public bool? IsLocal { get; set; }

		public string Jsonp { get; set; }

		public string JsonpCallback { get; set; }

		public string MimeType { get; set; }

		public string Password { get; set; }

		public bool? ProcessData { get; set; }

		public string ScriptCharset { get; set; }

		public string StatusCode { get; set; }

		public bool? Traditional { get; set; }

		public int? Timeout { get; set; }

		public string UserName { get; set; }

		public bool IsSaveMode { get; set; }

		public AjaxObject(IWidget widget)
			: base(widget)
		{
		}

		protected override void Serialize(IDictionary<string, object> json)
		{
			if (base.Url.HasValue())
			{
				json["url"] = base.Url;
			}
			if (base.Method.HasValue)
			{
				json["type"] = base.Method;
			}
			if (DataType.HasValue)
			{
				json["dataType"] = DataType;
			}
			if (ContentType.HasValue())
			{
				json["contentType"] = ContentType;
			}
		}
	}
}
