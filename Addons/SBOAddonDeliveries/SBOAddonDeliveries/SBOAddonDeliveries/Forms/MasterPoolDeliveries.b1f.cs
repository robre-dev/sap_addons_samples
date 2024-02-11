// -----------------------------------------------------------------------
// <copyright file="MasterPoolDeliveries.b1f.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Forms
{
    using System;
    using System.Collections.Generic;
    using SAPbouiCOM.Framework;
    using SBOAddonDeliveries.ObjectModels;

    [FormAttribute("SBOAddonDeliveries.Forms.MasterPoolDeliveries", "Forms/MasterPoolDeliveries.b1f")]
    public class MasterPoolDeliveries : UserFormBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MasterPoolDeliveries"/> class.
        /// </summary>
        public MasterPoolDeliveries()
        {
        }

        private SAPbouiCOM.Matrix oMatrixPoolDeliveries;
        private SAPbouiCOM.DataTable dtSOPending;
        private string querySalesOrders = string.Empty;
        private SAPbouiCOM.Button oButtonGenerate;
        private SAPbouiCOM.EditText oEditCardCode;
        private SAPbouiCOM.StaticText staticText0;
        private SAPbouiCOM.Button obuttonFilter;

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.oMatrixPoolDeliveries = (SAPbouiCOM.Matrix) this.GetItem("oMPD").Specific;
            this.oMatrixPoolDeliveries.DatasourceLoadAfter += new SAPbouiCOM._IMatrixEvents_DatasourceLoadAfterEventHandler(this.Matrix0_DatasourceLoadAfter);
            this.oButtonGenerate = (SAPbouiCOM.Button) this.GetItem("Item_1").Specific;
            this.oButtonGenerate.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.oEditCardCode = (SAPbouiCOM.EditText) this.GetItem("oECC").Specific;
            this.staticText0 = (SAPbouiCOM.StaticText) this.GetItem("Item_3").Specific;
            this.obuttonFilter = (SAPbouiCOM.Button) this.GetItem("btnFCC").Specific;
            this.obuttonFilter.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button1_ClickBefore);
            this.OnCustomInitialize();
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.CloseBefore += new SAPbouiCOM.Framework.FormBase.CloseBeforeHandler(this.Form_CloseBefore);
            this.LoadAfter += new SAPbouiCOM.Framework.FormBase.LoadAfterHandler(this.Form_LoadAfter);
            this.ActivateAfter += new ActivateAfterHandler(this.Form_ActivateAfter);
        }

        private void MasterPoolDeliveries_CloseAddonsForm(object sender, EventArgs e)
        {
           this.UIAPIRawForm.Close();
        }

        private void OnCustomInitialize()
        {
            // Initialice Elments
            this.dtSOPending = this.UIAPIRawForm.DataSources.DataTables.Item("DT_SO");
            this.querySalesOrders = Properties.Settings.Default.querySalesOrders.Replace("param", this.oEditCardCode.Value);

            // Filter Business Partner with only Customers
            var cFLOCRD = this.UIAPIRawForm.ChooseFromLists.Item("CFL_OCRD");
            var oConditionsCFL = cFLOCRD.GetConditions();
            var oConditionCFL = oConditionsCFL.Add();

            oConditionCFL.Alias = "CardType";
            oConditionCFL.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            oConditionCFL.CondVal = "C";

            cFLOCRD.SetConditions(oConditionsCFL);

            // First Load Data
            this.LoadData();
        }

        private void Button1_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            this.querySalesOrders = Properties.Settings.Default.querySalesOrders.Replace("param", this.oEditCardCode.Value);
            this.LoadData();
        }

        private void LoadData()
        {
            this.dtSOPending.ExecuteQuery(this.querySalesOrders);

            this.oMatrixPoolDeliveries.Columns.Item("#").DataBind.Bind("DT_SO", "#");
            this.oMatrixPoolDeliveries.Columns.Item("Col_0").DataBind.Bind("DT_SO", "Select");
            this.oMatrixPoolDeliveries.Columns.Item("Col_1").DataBind.Bind("DT_SO", "DocEntry");
            this.oMatrixPoolDeliveries.Columns.Item("Col_2").DataBind.Bind("DT_SO", "CardCode");
            this.oMatrixPoolDeliveries.Columns.Item("Col_3").DataBind.Bind("DT_SO", "CardName");
            this.oMatrixPoolDeliveries.Columns.Item("Col_4").DataBind.Bind("DT_SO", "DocDueDate");
            this.oMatrixPoolDeliveries.Columns.Item("Col_5").DataBind.Bind("DT_SO", "DocTotal");
            this.oMatrixPoolDeliveries.LoadFromDataSource();
        }

        private List<SelectedsSO> GetSelectedSO()
        {
            List<SelectedsSO> selectedsSO = new List<SelectedsSO>();
            for (int i = 0; i < this.dtSOPending.Rows.Count; i++)
            {
                if (Convert.ToString(this.dtSOPending.GetValue("Select", i)) == "Y")
                {
                    selectedsSO.Add(new SelectedsSO
                    {
                        DocEntry = Convert.ToInt32(this.dtSOPending.GetValue("DocEntry", i)),
                        Select = true,
                    });
                }
            }

            return selectedsSO;
        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            this.oMatrixPoolDeliveries.FlushToDataSource();

            var selectedsSO = this.GetSelectedSO();

            ProcessDeliveries form = new ProcessDeliveries(selectedsSO);
            form.CloseAddonsForm += this.Form_CloseAddonsForm;
            form.CloseFormToRefresh += this.Form_CloseFormToRefresh;
            form.Show();
        }

        private void Form_CloseFormToRefresh(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void Form_CloseAddonsForm(object sender, EventArgs e)
        {
           this.UIAPIRawForm.Close();
        }

        private void Form_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
        }

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
        }

        private void Matrix0_DatasourceLoadAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
        }

        private void Form_ActivateAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            Application.SBO_Application.Menus.Item("1300").Activate();
        }
    }
}
