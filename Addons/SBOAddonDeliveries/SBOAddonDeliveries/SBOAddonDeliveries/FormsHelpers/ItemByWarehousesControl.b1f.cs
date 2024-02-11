// -----------------------------------------------------------------------
// <copyright file="ItemByWarehousesControl.b1f.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.FormsHelpers
{
    using System;
    using SAPbouiCOM.Framework;

    [FormAttribute("SBOAddonDeliveries.FormsHelpers.ItemByWarehousesControl", "FormsHelpers/ItemByWarehousesControl.b1f")]
    public class ItemByWarehousesControl : UserFormBase
    {
        private SAPbouiCOM.DataTable dtWhs;
        private string queryItemByWarehouses;
        private bool isLoad;

        public ItemByWarehousesControl(string itemCode)
        {
            this.oEditItemCode.Value = itemCode;
            this.isLoad = true;
            this.OnCustomInitialize();
        }

        public string WhsCode { get; set; }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.staticText0 = (SAPbouiCOM.StaticText) this.GetItem("Item_0").Specific;
            this.staticText1 = (SAPbouiCOM.StaticText) this.GetItem("Item_1").Specific;
            this.oEditItemCode = (SAPbouiCOM.EditText) this.GetItem("Item_2").Specific;
            this.oEditItemName = (SAPbouiCOM.EditText) this.GetItem("Item_3").Specific;
            this.oLinkedButtonItemCode = (SAPbouiCOM.LinkedButton) this.GetItem("Item_4").Specific;
            this.oMatrixItemByWarehouses = (SAPbouiCOM.Matrix) this.GetItem("Item_5").Specific;
            this.oButtonChoose = (SAPbouiCOM.Button) this.GetItem("Item_6").Specific;
            this.oButtonChoose.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.oButtonCancel = (SAPbouiCOM.Button) this.GetItem("2").Specific;
            this.OnCustomInitialize();
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.LoadAfter += new LoadAfterHandler(this.Form_LoadAfter);
        }

        private SAPbouiCOM.StaticText staticText0;

        private void OnCustomInitialize()
        {
            // Initialice Elments
            this.dtWhs = this.UIAPIRawForm.DataSources.DataTables.Item("DT_Whs");
            this.queryItemByWarehouses = Properties.Settings.Default.queryItemByWarehouses.Replace("param", this.oEditItemCode.Value);
            if (this.isLoad)
            {
                this.LoadData();
            }
        }

        private SAPbouiCOM.StaticText staticText1;
        private SAPbouiCOM.EditText oEditItemCode;
        private SAPbouiCOM.EditText oEditItemName;
        private SAPbouiCOM.LinkedButton oLinkedButtonItemCode;
        private SAPbouiCOM.Matrix oMatrixItemByWarehouses;
        private SAPbouiCOM.Button oButtonChoose;
        private SAPbouiCOM.Button oButtonCancel;

        private void LoadData()
        {
            this.dtWhs.ExecuteQuery(this.queryItemByWarehouses);

            this.oMatrixItemByWarehouses.Columns.Item("#").DataBind.Bind("DT_Whs", "#");
            this.oMatrixItemByWarehouses.Columns.Item("Col_0").DataBind.Bind("DT_Whs", "WhsCode");
            this.oMatrixItemByWarehouses.Columns.Item("Col_1").DataBind.Bind("DT_Whs", "OnHand");
            this.oMatrixItemByWarehouses.Columns.Item("Col_2").DataBind.Bind("DT_Whs", "IsCommited");
            this.oMatrixItemByWarehouses.Columns.Item("Col_3").DataBind.Bind("DT_Whs", "OnOrder");
            this.oMatrixItemByWarehouses.Columns.Item("Col_4").DataBind.Bind("DT_Whs", "IsAvaible");

            this.oMatrixItemByWarehouses.LoadFromDataSource();

            if (this.dtWhs.Rows.Count > 0)
            {
                this.oEditItemName.Value = Convert.ToString(this.dtWhs.GetValue("ItemName", 0));
            }

            this.oMatrixItemByWarehouses.Columns.Item("Col_1").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            this.oMatrixItemByWarehouses.Columns.Item("Col_2").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            this.oMatrixItemByWarehouses.Columns.Item("Col_3").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
            this.oMatrixItemByWarehouses.Columns.Item("Col_4").ColumnSetting.SumType = SAPbouiCOM.BoColumnSumType.bst_Auto;
        }

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            var selectedRow = this.oMatrixItemByWarehouses.GetNextSelectedRow();
            if (selectedRow == -1)
            {
                Application.SBO_Application.MessageBox("you must select a row to delete it.");
                return;
            }

            this.WhsCode = Convert.ToString(this.dtWhs.GetValue("WhsCode", selectedRow - 1));
            this.UIAPIRawForm.Close();
        }
    }
}
