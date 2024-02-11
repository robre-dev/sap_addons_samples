// -----------------------------------------------------------------------
// <copyright file="SearchBusinessPartner.b1f.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries.FormsHelpers
{
    using System;
    using SAPbouiCOM.Framework;

    [FormAttribute("SBOAddonDeliveries.Forms.SearchBusinessPartner", "FormsHelpers/SearchBusinessPartner.b1f")]
    public class SearchBusinessPartner : UserFormBase
    {

        private SAPbouiCOM.Button oButtonCancel;
        private SAPbouiCOM.Button oButtonChoose;
        private SAPbouiCOM.EditText oEditCardCode;
        private SAPbouiCOM.StaticText staticText0;
        private SAPbouiCOM.StaticText staticText1;
        private SAPbouiCOM.EditText oEditCardName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBusinessPartner"/> class.
        /// </summary>
        public SearchBusinessPartner()
        {
        }

        /// <summary>
        /// Gets or sets is CardCode field.
        /// </summary>
        public string CardCode { get; set; }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.oButtonChoose = (SAPbouiCOM.Button) this.GetItem("Item_0").Specific;
            this.oButtonChoose.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.oButtonCancel = (SAPbouiCOM.Button) this.GetItem("2").Specific;
            this.oEditCardCode = (SAPbouiCOM.EditText) this.GetItem("Item_2").Specific;
            this.staticText0 = (SAPbouiCOM.StaticText) this.GetItem("Item_3").Specific;
            this.staticText1 = (SAPbouiCOM.StaticText) this.GetItem("Item_4").Specific;
            this.oEditCardName = (SAPbouiCOM.EditText) this.GetItem("Item_5").Specific;
            Application.SBO_Application.ItemEvent += this.SBO_Application_ItemEvent;

            this.OnCustomInitialize();
        }

        private void SBO_Application_ItemEvent(string formUID, ref SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST && pVal.ItemUID == "Item_2")
            {
                SAPbouiCOM.IChooseFromListEvent oCFLEvento = null;
                oCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pVal;
                string sCFL_ID = null;
                sCFL_ID = oCFLEvento.ChooseFromListUID;

                SAPbouiCOM.ChooseFromList oCFL = null;
                oCFL = this.UIAPIRawForm.ChooseFromLists.Item(sCFL_ID);
                if (oCFLEvento.BeforeAction == false)
                {
                    SAPbouiCOM.DataTable oDataTable = null;
                    oDataTable = oCFLEvento.SelectedObjects;
                    try
                    {
                        this.oEditCardName.Value = System.Convert.ToString(oDataTable.GetValue("CardName", 0));
                        this.CardCode = System.Convert.ToString(oDataTable.GetValue(0, 0));
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
    }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {
            // Filter Business Partner with only Customers
            var cFLOCRD = this.UIAPIRawForm.ChooseFromLists.Item("CFL_OCRD");
            var oConditionsCFL = cFLOCRD.GetConditions();
            var oConditionCFL = oConditionsCFL.Add();

            oConditionCFL.Alias = "CardType";
            oConditionCFL.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            oConditionCFL.CondVal = "C";

            cFLOCRD.SetConditions(oConditionsCFL);
        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            this.CardCode = this.oEditCardCode.Value;
            Application.SBO_Application.ItemEvent -= this.SBO_Application_ItemEvent;
            this.UIAPIRawForm.Close();
        }

        private void LoadData() { }
    }
}
