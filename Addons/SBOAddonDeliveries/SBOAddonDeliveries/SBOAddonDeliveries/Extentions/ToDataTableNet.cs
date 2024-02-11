// -----------------------------------------------------------------------
// <copyright file="ToDataTableNet.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Extentions
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    public static class ToDataTableNetExtention
    {
        public static DataTable ToDataTableNet(this SAPbouiCOM.DataTable sAPDataTable)
        {

            // \ This function will take an SAP DataTable from the SAPbouiCOM library and convert it to a more
            // \ easily used ADO.NET datatable which can be used for data binding much easier.
            DataTable dtTable = new DataTable();
            DataColumn newCol;
            DataRow newRow;
            int totalColCount = sAPDataTable.Columns.Count;
            int colCount;

            try
            {
                for (colCount = 0; colCount <= totalColCount - 1; colCount++)
                {
                    newCol = new DataColumn(sAPDataTable.Columns.Item(colCount).Name);
                    dtTable.Columns.Add(newCol);
                }

                Parallel.For(0, sAPDataTable.Rows.Count - 1, i =>
                {

                    newRow = dtTable.NewRow();

                    // populate each column in the row we're creating
                    for (colCount = 0; colCount <= totalColCount - 1; colCount++)
                    {
                        newRow[sAPDataTable.Columns.Item(colCount).Name] = sAPDataTable.GetValue(colCount, i);
                    }

                    // Add the row to the datatable
                    dtTable.Rows.Add(newRow);
                });

                return dtTable;
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.ToString()+ " Error converting SAP DataTable to DataTable .Net");
                return null;
            }
        }
    }
}
