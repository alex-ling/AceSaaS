using System.Collections.Generic;

namespace Acesoft.Web.UI.Ajax
{
	public class DataSource : AjaxObject
	{
		public static readonly ScriptEvent Loader = new ScriptEvent("loader", "param,successCallback,errorCallback");

		public static readonly ScriptEvent LoadFilter = new ScriptEvent("loadFilter", "data");

		public static readonly ScriptEvent OnBeforeLoad = new ScriptEvent("onBeforeLoad", "param");

		public static readonly ScriptEvent OnLoadSuccess = new ScriptEvent("onLoadSuccess", "data,status,xhr");

		public static readonly ScriptEvent OnLoadError = new ScriptEvent("onLoadError", "xhr,status,error");

		public dynamic FormData { get; set; }

		public Filter? Mode { get; set; }

		public dynamic QueryParams { get; set; }

		public bool IsEdit { get; set; }

		public DataSource(IWidget widget)
			: base(widget)
		{
			ScriptEvent.Regist(base.Events, OnLoadError, "AX.ajaxerror");
		}

		protected override void Serialize(IDictionary<string, object> json)
		{
			base.Serialize(json);
			if (base.Method.HasValue)
			{
				json["method"] = base.Method;
			}
			if (Mode.HasValue)
			{
				json["mode"] = Mode;
			}
			if (QueryParams != null)
			{
				json["queryParams"] = (object)QueryParams;
			}
			json["isEidt"] = IsEdit;
		}
	}
}
