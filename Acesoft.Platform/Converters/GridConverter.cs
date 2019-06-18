using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Acesoft.Data;
using Acesoft.Data.SqlMapper;

namespace Acesoft.Platform
{
    public class GridConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(GridResponse).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            GridResponse res = (GridResponse)value;
            bool isTree = ((SqlMap)res.Map).Params.GetValue("istree", false);
            bool num = isTree && res.Request.Id.HasValue;
            if (!num)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("total");
                writer.WriteValue(res.Total);
                writer.WritePropertyName("rows");
            }
            writer.WriteStartArray();
            WriteJson(isTree, res, writer, serializer);
            writer.WriteEndArray();
            if (!num)
            {
                writer.WriteEndObject();
            }
        }

        private void WriteJson(bool isTree, GridResponse res, JsonWriter writer, JsonSerializer serializer)
        {
            DataTable columnData = null;

            var sqlMap = ((SqlMap)res.Map);
            var dsColumnData = sqlMap.Params.GetValue("columndata", "");
            var idField = sqlMap.Params.GetValue("idfield", "id");
            if (dsColumnData.HasValue())
            {
                var ids = new List<object>();
                foreach (DataRow row in res.Data.Rows)
                {
                    ids.Add(row[idField]);
                }

                var session = App.Context.RequestServices.GetService<ISession>();
                columnData = session.QueryDataTable(new RequestContext(dsColumnData).SetParam(new
                {
                    ids
                }));
            }

            foreach (DataRow row in res.Data.Rows)
            {
                writer.WriteStartObject();
                if (isTree)
                {
                    writer.WritePropertyName("_parentId");
                    writer.WriteDbValue(row["parentid"]);
                    if (row.Table.Columns.Contains("childs") && Convert.ToInt32(row["childs"]) > 0)
                    {
                        writer.WritePropertyName("state");
                        writer.WriteValue("closed");
                    }
                }
                if (columnData != null)
                {
                    var id = row[idField];
                    var rows = columnData.Select($"isnull(id,'{id}')='{id}'", "name");
                    foreach (var r in rows)
                    {
                        writer.WritePropertyName(r["name"].ToString());
                        writer.WriteValue(r["value"]);
                    }
                }
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (!column.ColumnName.StartsWith("_"))
                    {
                        writer.WritePropertyName(column.ColumnName);
                        writer.WriteDbValue(row[column]);
                    }
                }
                foreach (KeyValuePair<string, string> col in ((SqlMap)res.Map).Actions)
                {
                    writer.WritePropertyName(col.Key);
                    StringBuilder sb = new StringBuilder();
                    string[] array = col.Value.Split(',');
                    foreach (string action in array)
                    {
                        string field = action.Split('=')[0].Split('_')[0];
                        if (row.Table.Columns.Contains(field))
                        {
                            object val = row[field];
                            if (val == Convert.DBNull || (int)val < 1)
                            {
                                continue;
                            }
                        }
                        sb.Append("," + action);
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(0, 1);
                    }
                    writer.WriteValue(sb.ToString());
                }
                writer.WriteEndObject();
            }
        }
    }
}