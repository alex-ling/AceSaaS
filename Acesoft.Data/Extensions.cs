using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    public static class Extensions
    {
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
                        return obj.ToString().ToObject<T>();
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
