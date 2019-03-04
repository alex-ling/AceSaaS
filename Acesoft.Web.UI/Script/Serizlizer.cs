using Acesoft.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;

namespace Acesoft.Web.UI.Script
{
	public class Serizlizer : IScriptSerializer
	{
		public static readonly IScriptSerializer Default = new Serizlizer();

		public static readonly IScriptSerializer Scriptor = new ScriptSerializer();

		public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings();

		public virtual string Initialize(string scripts)
		{
			return "$(function(){" + scripts + "});";
		}

		public virtual string InitializeFor(string id, string widget, IDictionary<string, object> options = null)
		{
			string text = (options == null) ? "" : Serialize(options, true);
			return Initialize("$('#" + id + "')." + widget + "({" + text + "});");
		}

		public virtual string Serialize(object value)
		{
			return SerializeHelper.ToJson(value, SerializerSettings);
		}

		public virtual string Serialize(string str, bool addQuotes = true)
		{
			string text = addQuotes ? "'" : "";
			return text + HttpUtility.JavaScriptStringEncode(str) + text;
		}

		public virtual string Serialize(IDictionary<string, object> options, bool removeQutes = true)
		{
			return SerializeHelper.ToJson(options);
		}
	}
}
