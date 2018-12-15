using Acesoft.NetCore.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text;

namespace Acesoft.Data
{
    public static class DataTableEx
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

        public static object ToObject(this DataRow dr, Type type, int index = 1)
        {
            if (type.FullName == "System.Object")
            {
                return dr.ToDynamic(index);
            }
            else
            {
                var obj = Dynamic.GetInstanceCreator(type)();
                if (obj is IEntityDto)
                {
                    ((IEntityDto)obj).Load(dr);
                }
                else
                {
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        var p = type.GetProperty(dc.ColumnName);
                        var val = dr[dc];
                        if (p != null && val != Convert.DBNull)
                        {
                            Dynamic.GetPropertySetter(p)(obj, val);
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

        public static dynamic ToDynamic(this DataRow dr, int index = 1)
        {
            return new DynamicRow(dr, index);
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
    }
}
