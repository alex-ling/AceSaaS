using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Acesoft.Util
{
    public static class SerializeHelper
    {
        public static byte[] ToBinary(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object FromBinary(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                var bf = new BinaryFormatter();
                return bf.Deserialize(ms);
            }
        }

        public static T FromBinary<T>(byte[] data)
        {
            return (T)FromBinary(data);
        }

        public static string ToJson(object obj, JsonSerializerSettings settings = null)
        {
            if (settings != null)
            {
                return JsonConvert.SerializeObject(obj, settings);
            }
            return JsonConvert.SerializeObject(obj, new LongConverter());
        }

        public static JObject FromJson(string json, JsonSerializerSettings settings = null)
        {
            return (JObject)JsonConvert.DeserializeObject(json, settings);
        }

        public static T FromJson<T>(string json, JsonSerializerSettings settings = null)
        {
            return (T)JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static byte[] ToJsonBytes(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var jx = new DataContractJsonSerializer(obj.GetType());
                jx.WriteObject(ms, obj);
                return ms.ToArray();
            }
        }

        public static T FromJson<T>(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                var jx = new DataContractJsonSerializer(typeof(T));
                return (T)jx.ReadObject(ms);
            }
        }

        public static byte[] ToXml(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(obj.GetType());
                xs.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T FromXml<T>(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Position = 0;
                var xs = new XmlSerializer(typeof(T));
                return (T)xs.Deserialize(ms);
            }
        }
    }
}
