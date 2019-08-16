using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Acesoft
{
    public class LongConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long val2 = 0L;
            if (value is long?)
            {
                long? vNull = value as long?;
                if (!vNull.HasValue)
                {
                    writer.WriteRaw("null");
                    return;
                }
                val2 = vNull.Value;
            }
            else
            {
                val2 = (long)value;
            }
            if (val2 > 9007199254740992L)
            {
                writer.WriteValue(val2.ToString());
            }
            else
            {
                writer.WriteValue(val2);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            if (!(typeof(long) == objectType))
            {
                return typeof(long?) == objectType;
            }
            return true;
        }
    }
}
