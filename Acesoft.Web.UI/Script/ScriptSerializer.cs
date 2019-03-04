using Acesoft.Web.UI.Ajax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.UI.Script
{
	public class ScriptSerializer : Serizlizer
	{
		public override string Serialize(IDictionary<string, object> options, bool removeQutes = true)
		{
			if (options == null || options.Count == 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, object> option in options)
			{
				stringBuilder.Append(",").Append(option.Key).Append(":");
				AppendObject(stringBuilder, option.Value);
			}
			if ((stringBuilder.Length >= 1) & removeQutes)
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		private void AppendObjects(StringBuilder sb, IEnumerable objs)
		{
			sb.Append("[");
			int length = sb.Length;
			foreach (object obj in objs)
			{
				AppendObject(sb, obj);
				sb.Append(",");
			}
			if (sb.Length > length)
			{
				sb.Remove(sb.Length - 1, 1);
			}
			sb.Append("]");
		}

		private void AppendObject(StringBuilder sb, object value)
		{
			JToken jToken;
			string str;
			StringBuilder stringBuilder;
			ScriptHandler scriptHandler;
			WidgetBase widgetBase;
			IDictionary<string, object> options;
			IEnumerable objs;
			if ((jToken = (value as JToken)) != null)
			{
				sb.Append(jToken.ToString(Formatting.None, Array.Empty<JsonConverter>()));
			}
			else if ((str = (value as string)) != null)
			{
				sb.Append(Serialize(str, true));
			}
			else if (value is Enum)
			{
				sb.Append(Serialize(value.ToString().ToLower(), true));
			}
			else if ((stringBuilder = (value as StringBuilder)) != null)
			{
				sb.Append(stringBuilder.ToString());
			}
			else if ((scriptHandler = (value as ScriptHandler)) != null)
			{
				sb.Append(scriptHandler.ToString());
			}
			else if ((widgetBase = (value as WidgetBase)) != null)
			{
				sb.Append("{");
				sb.Append(Serialize(widgetBase.HtmlBuilder.BuildOptions(), true));
				sb.Append("}");
			}
			else if ((options = (value as IDictionary<string, object>)) != null)
			{
				sb.Append(Serialize(options, true));
			}
			else if ((objs = (value as IEnumerable)) != null)
			{
				AppendObjects(sb, objs);
			}
			else
			{
				sb.Append(Serialize(value));
			}
		}
	}
}
