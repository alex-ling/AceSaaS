using Acesoft.Web.UI.Script;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Acesoft.Web.UI.Charts
{
	public class OptionConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof(Option).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Option option = (Option)value;
			IScriptSerializer scriptor = Serizlizer.Scriptor;
			writer.WriteStartObject();
			StringBuilder stringBuilder = new StringBuilder();
			if (option.Title != null)
			{
				stringBuilder.Append("title:{");
				stringBuilder.Append(scriptor.Serialize(option.Title.Options, true));
				stringBuilder.Append("},");
			}
			if (option.Legend != null)
			{
				stringBuilder.Append("legend:{");
				stringBuilder.Append(scriptor.Serialize(option.Legend.Options, true));
				stringBuilder.Append("},");
			}
			if (option.XAxis != null)
			{
				stringBuilder.Append("xAxis:{");
				stringBuilder.Append(scriptor.Serialize(option.XAxis.Options, true));
				stringBuilder.Append("},");
			}
			if (option.YAxis != null)
			{
				stringBuilder.Append("yAxis:{");
				stringBuilder.Append(scriptor.Serialize(option.YAxis.Options, true));
				stringBuilder.Append("},");
			}
			stringBuilder.Append("series:[");
			foreach (Series item in option.Series)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("type:'" + item.Type.ToString() + "'");
				stringBuilder.Append(scriptor.Serialize(item.Options, false));
				stringBuilder.Append("},");
			}
            stringBuilder.Remove();
			stringBuilder.Append("]");
			stringBuilder.Append(scriptor.Serialize(option.Options, false));
			writer.WriteRaw(stringBuilder.ToString());
			writer.WriteEndObject();
		}
	}
}
