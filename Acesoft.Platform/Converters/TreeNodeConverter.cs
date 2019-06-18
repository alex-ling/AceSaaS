using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Newtonsoft.Json;
using Acesoft.Data;

namespace Acesoft.Platform
{
    public class TreeNodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(TreeNode).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var root = (TreeNode)value;

            writer.WriteStartArray();
            writer.WriteStartObject();
            writer.WritePropertyName("id");
            writer.WriteValue(root.Id);
            writer.WritePropertyName("text");
            writer.WriteValue(root.Text);
            if (root.IconCls.HasValue())
            {
                writer.WritePropertyName("iconCls");
                writer.WriteValue(root.IconCls);
            }

            writer.WritePropertyName("children");
            WriteJson(writer, root.Children);

            writer.WriteEndObject();
            writer.WriteEndArray();
        }

        private void WriteJson(JsonWriter writer, List<TreeNode> nodes)
        {
            writer.WriteStartArray();

            foreach (var node in nodes)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("id");
                writer.WriteValue(node.Id);
                writer.WritePropertyName("text");
                writer.WriteValue(node.Text);

                if (node.Checked.HasValue)
                {
                    writer.WritePropertyName("checked");
                    writer.WriteValue(node.Checked.Value);
                }
                if (node.IconCls.HasValue())
                {
                    writer.WritePropertyName("iconCls");
                    writer.WriteDbValue(node.IconCls);
                }
                if (node.Attributes.Any())
                {
                    writer.WritePropertyName("attributes");
                    writer.WriteStartObject();
                    foreach (var attr in node.Attributes)
                    {
                        writer.WritePropertyName(attr.Key);
                        writer.WriteDbValue(attr.Value);
                    }
                    writer.WriteEndObject();
                }

                if (node.State.HasValue)
                {
                    writer.WritePropertyName("state");
                    writer.WriteValue(node.State.ToString());
                }
                else if (node.Children.Any())
                {
                    writer.WritePropertyName("children");
                    WriteJson(writer, node.Children);
                }

                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}