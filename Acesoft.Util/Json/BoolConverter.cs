using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Acesoft
{
    public class BoolConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(bool) == objectType;
        }
    }
}
