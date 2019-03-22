using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Newtonsoft.Json;
using Acesoft.Data;

namespace Acesoft.Platform
{
    public class TreeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(TreeResponse).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TreeResponse res = (TreeResponse)value;
            if (res.Request.RootName.HasValue())
            {
                writer.WriteStartArray();
                writer.WriteStartObject();
                writer.WritePropertyName("id");
                writer.WriteValue(res.Request.RootId ?? "");
                writer.WritePropertyName("text");
                writer.WriteValue(res.Request.RootName);
                if (res.Request.RootIcon.HasValue())
                {
                    writer.WritePropertyName("iconCls");
                    writer.WriteValue(res.Request.RootIcon);
                }
                writer.WritePropertyName("children");
            }

            IEnumerable<DataRow> rows = null;
            if (res.Request.Id.HasValue || !res.Data.Columns.Contains("parentid"))
            {
                rows = res.Data.Rows.Cast<DataRow>();
            }
            else if (res.Request.RootId.HasValue())
            {
                rows = res.Data.Select($"parentid='{res.Request.RootId}'", "orderno");
            }
            else
            {
                rows = res.Data.Select("parentid is null", "orderno");
            }
            WriteJson(writer, rows, res.Request);

            if (res.Request.RootName.HasValue())
            {
                writer.WriteEndObject();
                writer.WriteEndArray();
            }
        }

        private void WriteJson(JsonWriter writer, IEnumerable<DataRow> rows, TreeRequest request)
        {
            writer.WriteStartArray();

            if (request != null && request.NullSelect)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("id");
                writer.WriteValue("");
                writer.WritePropertyName("text");
                writer.WriteValue("　");
                writer.WriteEndObject();
            }

            foreach (DataRow row in rows)
            {
                var id = row[0].ToString();
                var text = row[1].ToString();
                if (row.Table.Columns.Contains("title"))
                {
                    text = $"<span title=\"{row["title"]}\">{text}</span>";
                }

                writer.WriteStartObject();
                writer.WritePropertyName("id");
                writer.WriteValue(id);
                writer.WritePropertyName("text");
                writer.WriteValue(text);

                if (row.Table.Columns.Contains("checked"))
                {
                    writer.WritePropertyName("checked");
                    writer.WriteValue(row["checked"] != Convert.DBNull && Convert.ToBoolean(row["checked"]));
                }
                if (row.Table.Columns.Contains("icon"))
                {
                    writer.WritePropertyName("iconCls");
                    writer.WriteDbValue(row["icon"]);
                }
                if (row.Table.Columns.Count > 4)
                {
                    writer.WritePropertyName("attributes");
                    writer.WriteStartObject();
                    for (int i = 4; i < row.Table.Columns.Count; i++)
                    {
                        DataColumn col = row.Table.Columns[i];
                        if (!(col.ColumnName == "checked") && !(col.ColumnName == "icon"))
                        {
                            writer.WritePropertyName(col.ColumnName);
                            writer.WriteDbValue(row[col]);
                        }
                    }
                    writer.WriteEndObject();
                }

                DataRow[] childRows = null;
                var childs = row.Table.Columns.Contains("childs") ? Convert.ToInt32(row["childs"]) : 0;
                if (row.Table.Columns.Contains("parentid"))
                {
                    childRows = row.Table.Select("parentid='" + id + "'", "orderno");
                }
                if (childs > 0 && (childRows == null || (childRows != null && childRows.Length == 0)))
                {
                    writer.WritePropertyName("state");
                    writer.WriteValue("closed");
                }
                else if (childRows != null && childRows.Length != 0)
                {
                    writer.WritePropertyName("children");
                    WriteJson(writer, childRows, null);
                }

                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}