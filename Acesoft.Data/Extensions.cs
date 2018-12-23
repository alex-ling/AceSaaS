using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace Acesoft.Data
{
    public static class Extensions
    {
        public static IList<T> ToObject<T>(this DataTable dt)
        {
            return (IList<T>)ToObject(dt, typeof(T));
        }

        public static T ToObject<T>(this DataRow dr, int index = 1)
        {
            return (T)ToObject(dr, typeof(T), index);
        }

        public static IList ToObject(this DataTable dt, Type type)
        {
            var index = 1;
            var list = type.MakeEmptyList();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr.ToObject(type, index++));
            }
            return list;
        }

        public static object ToObject(this DataRow row, Type type, int index = 1)
        {
            if (type.FullName == "System.Object")
            {
                return row.ToDynamic(index);
            }
            else
            {
                var obj = Dynamic.GetInstanceCreator(type)();
                if (obj is IEntityDto)
                {
                    ((IEntityDto)obj).Load(row);
                }
                else
                {
                    foreach (var property in Dynamic.GetProperties(type))
                    {
                        if (row.Table.Columns.Contains(property.Name))
                        {
                            var val = row[property.Name];
                            if (val != Convert.DBNull)
                            {
                                Dynamic.GetPropertySetter(property)(obj, val);
                            }
                        }
                    }
                }
                return obj;
            }
        }

        public static IEnumerable<dynamic> ToDynamic(this DataTable dt)
        {
            var list = new List<dynamic>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i].ToDynamic(i + 1));
            }
            return list;
        }

        public static dynamic ToDynamic(this DataRow row, int index = 1)
        {
            return new DynamicRow(row, index);
        }

        private sealed class DynamicRow : DynamicObject
        {
            private readonly DataRow row;
            private int index;

            internal DynamicRow(DataRow row, int index)
            {
                this.row = row;
                this.index = index;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var rv = row.Table.Columns.Contains(binder.Name) || binder.Name == "_index";
                result = null;
                if (rv)
                {
                    if (binder.Name == "_index")
                    {
                        result = index;
                    }
                    else
                    {
                        var obj = row[binder.Name];
                        if (obj != Convert.DBNull)
                        {
                            result = obj;
                        }
                        else if (row.Table.Columns[binder.Name].DataType.FullName == "System.String")
                        {
                            result = "";
                        }
                    }
                }
                return rv;
            }
        }

        public static T GetValue<T>(this DataRow row, string name, T defaultValue)
        {
            if (row.Table.Columns.Contains(name))
            {
                var obj = row[name];
                if (obj != Convert.DBNull)
                {
                    if (obj is T)
                    {
                        return (T)obj;
                    }
                    else
                    {
                        return obj.ToObject<T>();
                    }
                }
            }

            return defaultValue;
        }

        public static DataTable ToDataTable(this IDataReader dr)
        {
            DataTable schema = dr.GetSchemaTable();
            DataTable dt = new DataTable();
            dt.TableName = "table";
            for (int i = 0; i < dr.FieldCount; i++)
            {
                dt.Columns.Add(dr.GetName(i), dr.GetFieldType(i));
            }

            while (dr.Read())
            {
                DataRow row = dt.NewRow();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    row[i] = dr.GetValue(i);
                }
                dt.Rows.Add(row);
            }
            dr.Close();

            return dt;
        }

        public static DataSet ToDataSet(this IDataReader dr)
        {
            DataSet ds = new DataSet();
            int ix = 0;

            do
            {
                DataTable schema = dr.GetSchemaTable();
                DataTable dt = new DataTable();
                dt.TableName = "table" + (ix++ > 0 ? ix.ToString() : "");
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    dt.Columns.Add(dr.GetName(i), dr.GetFieldType(i));
                }

                while (dr.Read())
                {
                    DataRow row = dt.NewRow();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        row[i] = dr.GetValue(i);
                    }
                    dt.Rows.Add(row);
                }

                ds.Tables.Add(dt);
            }
            while (dr.NextResult());
            dr.Close();

            return ds;
        }
    }
}
