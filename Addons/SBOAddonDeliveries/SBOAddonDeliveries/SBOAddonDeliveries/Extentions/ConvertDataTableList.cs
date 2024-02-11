// -----------------------------------------------------------------------
// <copyright file="ConvertDataTableList.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Extentions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public static class ConvertDataTableList
    {
        public static List<T> ConvertDataTable<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }

            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {

                        switch (pro.PropertyType.FullName)
                        {
                            case "System.Boolean":
                                pro.SetValue(obj,  Convert.ToString(dr[column.ColumnName])=="Y", null);
                                break;
                            case "System.Int32":
                                pro.SetValue(obj, Convert.ToInt32(dr[column.ColumnName]) , null);
                                break;
                            default:
                                break;
                        }
                    }

                    else
                        continue;
                }
            }

            return obj;
        }
    }
}
