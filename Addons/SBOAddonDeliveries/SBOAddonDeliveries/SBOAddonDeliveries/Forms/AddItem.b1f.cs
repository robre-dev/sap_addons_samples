// -----------------------------------------------------------------------
// <copyright file="AddItem.b1f.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.Forms
{
    using System;
    using SAPbouiCOM.Framework;
    using SBOAddonDeliveries.FormsHelpers;

    [FormAttribute("SBOAddonDeliveries.Forms.AddItem", "Forms/AddItem.b1f")]
    public class AddItem : UserFormBase
    {
        private ItemByWarehousesControl itemByWarehousesControl;
        private SearchBusinessPartner searchBusinessPartner;
        private SAPbouiCOM.EditText oEditQuantity;
        private SAPbouiCOM.StaticText staticText3;
        private SAPbouiCOM.StaticText staticText4;
        private SAPbouiCOM.EditText oEditPrice;
        private SAPbouiCOM.Button oButtonSearchCustomer;
        private SAPbouiCOM.Button oButtonSearchWarehouse;
        private SAPbouiCOM.Button button0;
        private SAPbouiCOM.Button button1;

        private SAPbouiCOM.EditText editCardCode;
        private SAPbouiCOM.StaticText staticText0;
        private SAPbouiCOM.EditText editText1;
        private SAPbouiCOM.StaticText staticText1;
        private SAPbouiCOM.StaticText staticText2;
        private SAPbouiCOM.EditText oEditWhsCode;

        public AddItem()
        {
        }

        public string CardCode { get; set; }

        public string ItemCode { get; set; }

        public string WhsCode { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.editCardCode = ((SAPbouiCOM.EditText)(this.GetItem("Item_0").Specific));
            this.staticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.editText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_2").Specific));
            this.staticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.staticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.oEditWhsCode = ((SAPbouiCOM.EditText)(this.GetItem("Item_5").Specific));
            this.oEditWhsCode.KeyDownAfter += new SAPbouiCOM._IEditTextEvents_KeyDownAfterEventHandler(this.EditText2_KeyDownAfter);
            this.oEditWhsCode.DoubleClickAfter += new SAPbouiCOM._IEditTextEvents_DoubleClickAfterEventHandler(this.EditText2_DoubleClickAfter);
            this.oEditQuantity = ((SAPbouiCOM.EditText)(this.GetItem("Item_6").Specific));
            this.staticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.staticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.oEditPrice = ((SAPbouiCOM.EditText)(this.GetItem("Item_9").Specific));
            this.button0 = ((SAPbouiCOM.Button)(this.GetItem("addbtn").Specific));
            this.button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.oButtonSearchCustomer = ((SAPbouiCOM.Button)(this.GetItem("Item_10").Specific));
            this.oButtonSearchCustomer.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.OButtonSearchCustomer_ClickBefore);
            this.oButtonSearchWarehouse = ((SAPbouiCOM.Button)(this.GetItem("Item_11").Specific));
            this.oButtonSearchWarehouse.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.oButtonSearchWarehouse_ClickAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.LoadAfter += new SAPbouiCOM.Framework.FormBase.LoadAfterHandler(this.Form_LoadAfter);
            SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent += this.SBO_Application_ItemEvent;
            this.CloseBefore += new CloseBeforeHandler(this.Form_CloseBefore);
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);

        }

        private void ItemByWarehousesControl_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            if (!string.IsNullOrEmpty(this.itemByWarehousesControl.WhsCode))
            {
                this.oEditWhsCode.Value = this.itemByWarehousesControl.WhsCode;
            }

            bubbleEvent = true;
        }

        private void SBO_Application_ItemEvent(string formUID, ref SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {

            bubbleEvent = true;
        }

        private void OnCustomInitialize()
        {
        }

        private void EditText2_DoubleClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (string.IsNullOrEmpty(this.editText1.Value))
            {
                Application.SBO_Application.MessageBox("you must select an Item Code to select a warehouse.");
                return;
            }

            this.itemByWarehousesControl = new ItemByWarehousesControl(this.editText1.Value);
            this.itemByWarehousesControl.CloseBefore += this.ItemByWarehousesControl_CloseBefore;
            this.itemByWarehousesControl.Show();
        }

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
            if (string.IsNullOrEmpty(this.editCardCode.Value) || string.IsNullOrEmpty(this.editText1.Value) || string.IsNullOrEmpty(this.oEditWhsCode.Value) || string.IsNullOrEmpty(this.oEditQuantity.Value) || string.IsNullOrEmpty(this.oEditPrice.Value))
            {
                Application.SBO_Application.MessageBox("All fields are required.");
                return;
            }

            this.CardCode = this.editCardCode.Value;
            this.ItemCode = this.editText1.Value;
            this.WhsCode = this.oEditWhsCode.Value;
            this.Quantity = Convert.ToDouble(this.oEditQuantity.Value);
            this.Price = Convert.ToDouble(this.oEditPrice.Value);

            this.UIAPIRawForm.Close();
        }

        private void EditText2_KeyDownAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
        }

        private void Button1_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            if (string.IsNullOrEmpty(this.editText1.Value))
            {
                Application.SBO_Application.MessageBox("you must select an Item Code to select a warehouse.");
                return;
            }

            this.itemByWarehousesControl = new ItemByWarehousesControl(this.editText1.Value);
            this.itemByWarehousesControl.CloseBefore += this.ItemByWarehousesControl_CloseBefore;
            this.itemByWarehousesControl.Show();

        }

        private void OButtonSearchCustomer_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            this.searchBusinessPartner = new SearchBusinessPartner();
            this.searchBusinessPartner.CloseBefore += this.Form_CloseBefore;
            this.searchBusinessPartner.Show();

        }

        private void Form_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            if (!(this.searchBusinessPartner == null))
            {
                this.editCardCode.Value = this.searchBusinessPartner.CardCode;
            }
        }

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;

        }

        private void oButtonSearchWarehouse_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (string.IsNullOrEmpty(this.editText1.Value))
            {
                Application.SBO_Application.MessageBox("you must select an Item Code to select a warehouse.");
                return;
            }

            this.itemByWarehousesControl = new ItemByWarehousesControl(this.editText1.Value);
            this.itemByWarehousesControl.CloseBefore += this.ItemByWarehousesControl_CloseBefore;
            this.itemByWarehousesControl.Show();

        }
    }
}
