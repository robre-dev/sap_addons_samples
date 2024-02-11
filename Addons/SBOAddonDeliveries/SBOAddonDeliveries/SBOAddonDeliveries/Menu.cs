// -----------------------------------------------------------------------
// <copyright file="Menu.cs" company="Jorge Robredo">
// Copyright (c) Developed by Jorge Robredo. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SBOAddonDeliveries
{
    using System;
    using System.IO;
    using SBOAddonDeliveries.Forms;
    using SAPbouiCOM.Framework;

    public class Menu
    {
        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = (SAPbouiCOM.MenuCreationParams) Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams);
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "SBOAddonDeliveries";
            oCreationPackage.String = "Deliveries AddOn";
            oCreationPackage.Image = Directory.GetCurrentDirectory() + "\\Resources\\TIcon.png";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;

            oMenus = oMenuItem.SubMenus;

            try
            {
                // If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception e)
            {
            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("SBOAddonDeliveries");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "SBOAddonDeliveries.MasterDeliveries";
                oCreationPackage.String = "Create Deliveries";
                oCreationPackage.Image = string.Empty;
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { // Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "SBOAddonDeliveries.MasterDeliveries")
                {

                    MasterPoolDeliveries activeForm = new MasterPoolDeliveries();
                    activeForm.Show();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", string.Empty, string.Empty);
            }
        }
    }
}
