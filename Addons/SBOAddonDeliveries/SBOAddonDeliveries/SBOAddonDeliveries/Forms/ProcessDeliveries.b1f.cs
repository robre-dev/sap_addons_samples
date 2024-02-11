// -----------------------------------------------------------------------
// <copyright file="ProcessDeliveries.b1f.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SAPbouiCOM.Framework;
    using SBOAddonDeliveries.Core.Delivery;
    using SBOAddonDeliveries.ObjectModels;

    [FormAttribute("SBOAddonDeliveries.Forms.ProcessDeliveries", "Forms/ProcessDeliveries.b1f")]
    public class ProcessDeliveries : UserFormBase
    {
        public ProcessDeliveries(List<SelectedsSO> selectedsSO)
        {
            this.selectedsSO = selectedsSO;
            this.deliveryManager = new DeliveryManager(Program.Company);
            this.isLoad = true;
            this.OnCustomInitialize();
        }

        public event EventHandler CloseAddonsForm;

        public event EventHandler CloseFormToRefresh;

        public List<SelectedsSO> selectedsSO;
        private readonly DeliveryManager deliveryManager;
        private SAPbouiCOM.DataTable dtSODetails;
        private SAPbouiCOM.Matrix oMatrixProcessDeliveries;
        private string queryDetailsSalesOrders;
        private bool isLoad = false;
        private AddItem addItem;

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.oMatrixProcessDeliveries = (SAPbouiCOM.Matrix)this.GetItem("Item_0").Specific;
            this.oButtonAdd = (SAPbouiCOM.Button)this.GetItem("Item_1").Specific;
            this.oButtonAdd.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.oButtonRemove = (SAPbouiCOM.Button)this.GetItem("Item_2").Specific;
            this.oButtonRemove.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button1_ClickBefore);
            this.oButtonExecute = (SAPbouiCOM.Button)this.GetItem("Item_3").Specific;
            this.oButtonExecute.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button2_ClickBefore);
            this.OnCustomInitialize();
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ActivateAfter += new ActivateAfterHandler(this.Form_ActivateAfter);
        }

        protected void CloseAddonsFormEvent(object sender, EventArgs e)
        {
            // bubble the event up to the parent
            if (this.CloseAddonsForm != null)
            {
                this.CloseAddonsFormEvent(this, e);
            }
        }

        protected void CloseFormToRefreshEvent(object sender, EventArgs e)
        {
            // bubble the event up to the parent
            if (this.CloseFormToRefresh != null)
            {
                this.CloseFormToRefreshEvent(this, e);
            }
        }

        private void OnCustomInitialize()
        {
            // Initialice Elments
            if (this.isLoad)
            {
                this.dtSODetails = this.UIAPIRawForm.DataSources.DataTables.Item("DT_SODE");
                this.queryDetailsSalesOrders = Properties.Settings.Default.querySalesOrderDetails.Replace("param", string.Join(", ", this.selectedsSO.Select(l => l.DocEntry).ToList()));

                // First Load Data
                this.LoadData();
                Application.SBO_Application.Menus.Item("1300").Activate();
            }
        }

        private void LoadData()
        {
            this.dtSODetails.ExecuteQuery(this.queryDetailsSalesOrders);

            this.oMatrixProcessDeliveries.Columns.Item("#").DataBind.Bind("DT_SODE", "#");
            this.oMatrixProcessDeliveries.Columns.Item("Col_0").DataBind.Bind("DT_SODE", "DocEntry");
            this.oMatrixProcessDeliveries.Columns.Item("Col_1").DataBind.Bind("DT_SODE", "CardCode");
            this.oMatrixProcessDeliveries.Columns.Item("Col_2").DataBind.Bind("DT_SODE", "ItemCode");
            this.oMatrixProcessDeliveries.Columns.Item("Col_3").DataBind.Bind("DT_SODE", "WhsCode");
            this.oMatrixProcessDeliveries.Columns.Item("Col_4").DataBind.Bind("DT_SODE", "Quantity");
            this.oMatrixProcessDeliveries.Columns.Item("Col_5").DataBind.Bind("DT_SODE", "Price");
            this.oMatrixProcessDeliveries.Columns.Item("Col_6").DataBind.Bind("DT_SODE", "BaseLine");
            this.oMatrixProcessDeliveries.LoadFromDataSource();
        }

        private SAPbouiCOM.Button oButtonAdd;
        private SAPbouiCOM.Button oButtonRemove;
        private SAPbouiCOM.Button oButtonExecute;

        private void Button1_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            var selectedRow = this.oMatrixProcessDeliveries.GetNextSelectedRow();
            if (selectedRow == -1)
            {
                Application.SBO_Application.MessageBox("you must select a row to delete it.");
                return;
            }

            if (Application.SBO_Application.MessageBox("Do you want to delete the selected line ? ", 1, Btn1Caption: "Yes", Btn2Caption: "No") == 1)
            {
                this.oMatrixProcessDeliveries.DeleteRow(selectedRow);
            }

            this.oMatrixProcessDeliveries.FlushToDataSource();
        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            this.addItem = new AddItem();
            this.addItem.CloseBefore += this.AddItem_CloseBefore;
            this.addItem.Show();
        }

        private void AddItem_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            if (!string.IsNullOrEmpty(this.addItem.CardCode) && !string.IsNullOrEmpty(this.addItem.ItemCode) && !string.IsNullOrEmpty(this.addItem.WhsCode) && (this.addItem.Quantity > 0) && (this.addItem.Price > 0))
            {
                this.dtSODetails.Rows.Add(1);
                var indexRow = this.dtSODetails.Rows.Count - 1;

                this.dtSODetails.SetValue("#", indexRow, this.GetLastId() + 1);
                this.dtSODetails.SetValue("CardCode", indexRow, this.addItem.CardCode);
                this.dtSODetails.SetValue("ItemCode", indexRow, this.addItem.ItemCode);
                this.dtSODetails.SetValue("WhsCode", indexRow, this.addItem.WhsCode);
                this.dtSODetails.SetValue("Quantity", indexRow, this.addItem.Quantity);
                this.dtSODetails.SetValue("Price", indexRow, this.addItem.Price);

                this.oMatrixProcessDeliveries.LoadFromDataSource();
            }

            bubbleEvent = true;
        }

        private int GetLastId()
        {
            List<int> listNum = new List<int>();
            for (int i = 0; i < this.dtSODetails.Rows.Count; i++)
            {
                listNum.Add(Convert.ToInt32(this.dtSODetails.GetValue("#", i)));
            }

            return listNum.Max(l => l);
        }

        private List<DeliveryData> GetDataToProcess()
        {
            var listDataProcess = new List<DeliveryData>();
            for (int i = 0; i < this.dtSODetails.Rows.Count; i++)
            {
                listDataProcess.Add(new DeliveryData
                {
                    CardCode = Convert.ToString(this.dtSODetails.GetValue("CardCode", i)),
                    ItemCode = Convert.ToString(this.dtSODetails.GetValue("ItemCode", i)),
                    WhsCode = Convert.ToString(this.dtSODetails.GetValue("WhsCode", i)),
                    DocEntry = Convert.ToInt32(this.dtSODetails.GetValue("DocEntry", i)),
                    Price = Convert.ToDouble(this.dtSODetails.GetValue("Price", i)),
                    Quantity = Convert.ToInt32(this.dtSODetails.GetValue("Quantity", i)),
                    BaseLine = Convert.ToInt32(this.dtSODetails.GetValue("BaseLine", i)),
                });
            }

            return listDataProcess;
        }

        private void Form_ActivateAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            Application.SBO_Application.Menus.Item("1300").Activate();
        }

        private void Button2_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            var data = this.GetDataToProcess().Select(l => new
            DeliveryModel
            {
                CardCode = l.CardCode,
                DocEntry = l.DocEntry,
                ItemCode = l.ItemCode,
                Price = l.Price,
                Quantity = l.Quantity,
                WhsCode = l.WhsCode,
                BaseLine = l.BaseLine,
            }).ToList();
            try
            {
                var isAgrupate = Application.SBO_Application.MessageBox("you want to group by business partner?", Btn1Caption: "Yes", Btn2Caption: "No", Btn3Caption: "Cancel");
                if (isAgrupate != 3)
                {
                    var result = this.deliveryManager.ExecuteDelivery(data, isAgrupate == 1);

                    if (result.Passed)
                    {
                        Application.SBO_Application.SetStatusBarMessage("all rows added", IsError: false);
                        if (Application.SBO_Application.MessageBox("Do you want to continue processing more deliveries?", Btn1Caption: "Yes", Btn2Caption: "No") == 1)
                        {
                            this.CloseFormToRefresh?.Invoke(null, null);
                            this.UIAPIRawForm.Close();
                        }
                        else
                        {
                            this.CloseAddonsForm?.Invoke(null, null);
                            this.UIAPIRawForm.Close();
                        }
                    }
                    else
                    {
                        Application.SBO_Application.MessageBox(result.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", string.Empty, string.Empty);
            }
        }
    }
}
