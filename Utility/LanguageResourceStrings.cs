﻿/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  LanguageResourceStrings 
 * 
 *  serializable data model class 
 *  for the language infrastructure 
 *  holds all texts within the application
 *  
 *  to do:
 *  
 *  split the member definitions into 
 *  several parts for every viewmodel
 */
using System.Text;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Utility
{
    [Serializable]
    [XmlRoot("Language")]
    public class LanguageResourceStrings
    {

        // properties & fields
        #region properties

        public string Language { get; set; } = "";


        // billing views
        #region billing views

        public string IDF_AnnualBillings { get; set; } = "Annual Billing(s)";
        public string IDF_Billing { get; set; } = "Billing";
        public string IDF_BillingPeriodData { get; set; } = "Billing Data";
        public string IDF_BillingPeriodOptions { get; set; } = "Billing Options";
        public string IDF_Consumption { get; set; } = "Consumption";
        public string IDF_ConsumptionShare { get; set; } = "Heating\nShare";
        public string IDF_CombinedConsumption { get; set; } = "Heating\nCombined";
        public string IDF_ConsumptionPercentage { get; set; } = "Heating\n% ";

        public string IDF_ExtraCosts { get; set; } = "Extra\nCosts";
        public string IDF_FixedCosts { get; set; } = "Fixed";
        public string IDF_HeatingCosts { get; set; } = "Heating";
        public string IDF_TotalCosts { get; set; } = "Total\nCosts";
        public string IDF_Balance { get; set; } = "Balance";
        public string IDF_RoomData { get; set; } = "room data";
        public string IDF_RoomCostShares { get; set; } = "annual room costs";
        public string IDF_OtherCostsData { get; set; } = "other costs data";

        public string IDF_RoomHeatingUnits { get; set; } = "Heating\nRoom";
        public string IDF_CombinedRoomHeatingUnits { get; set; } = "All Rooms\nHeating Units";
        public string IDF_Percentage { get; set; } = "%";
        public string IDF_TotalConsumption { get; set; } = "Total";
        public string IDF_SharedConsumption { get; set; } = "Shared";
        public string IDF_TotalRooms { get; set; } = "Rooms";
        public string IDF_Year { get; set; } = "Year";
        

        public string BillingManagementInstructionText { get; set; }

        #endregion billing views


        // buttons and commands
        #region buttons and commands

        public string IDF_Back { get; set; } = "Back";
        public string IDF_NewBilling { get; set; } = "New Annual Billing";
        public string IDF_DeleteBilling { get; set; } = "Delete Annual Billing";
        public string IDF_PrintView { get; set; } = "Print";
        public string IDF_ShowCosts { get; set; } = "Costs"; 
        public string IDF_Other { get; set; } = "Other";
        public string IDF_Tenants { get; set; } = "Tenants";

        public string IDF_Raise { get; set; } = "Add Raise";
        public string IDF_NewRent { get; set; } = "New Rent";
        public string IDF_NewRentChange { get; set; } = "New Rent Change";

        
        public string IDF_DeleteRent { get; set; } = "Delete Rent";
        public string IDF_DeleteRentChange { get; set; } = "Delete Rent Change(s)";


        public string IDF_NewFlat { get; set; } = "New Flat";
        public string IDF_EditFlat { get; set; } = "Edit Flat";
        public string IDF_DeleteFlat { get; set; } = "Delete Flat";
        public string IDF_FlatSetup { get; set; } = "Flat";
        public string IDF_RoomSetup { get; set; } = "Rooms";
        public string IDF_Manual { get; set; } = "Manual";
        public string IDF_Settings { get; set; } = "Settings";
        public string IDF_FlatCosts { get; set; } = "Flat Costs Overview";
        public string IDF_RoomCosts { get; set; } = "Room Costs Overview";

        #endregion buttons and commands


        // flat and room views, general stuff
        #region flat and room views, general stuff

        public string IDF_StartDate { get; set; } = "Begin";
        public string IDF_EndDate { get; set; } = "End";

        public string IDF_ID { get; set; } = "ID";
        public string IDF_RoomName { get; set; } = "Room";
        public string IDF_RoomArea { get; set; } = "Area";

        public string IDF_MainWindowTitle { get; set; } = "Shared Living TransactionSum Calculator (SLCC)";

        public string IDF_Address { get; set; } = "Address";
        public string IDF_FlatArea { get; set; } = "Flat Area";
        public string IDF_Details { get; set; } = "Details";
        public string IDF_Rooms { get; set; } = "Rooms";

        public string IDF_RoomTenant { get; set; } = "Tenant";
        public string IDF_MovingIn { get; set; } = "Moving In";
        public string IDF_MovingOut { get; set; } = "Moving Out";
        public string IDF_TenantIsActive { get; set; } = "Is Active";
        public string IDF_TenantManagement { get; set; } = "Tenants";
        public string IDF_TenantSetup { get; set; } = "Tenants"; 
        public string IDF_ActiveTenantCount { get; set; } = "Active Tenants";

        public string IDF_SharedFlatArea { get; set; } = "Shared\nArea";

        public string IDF_RentedAreaShare { get; set; } = "Rented\nArea";
        public string IDF_CombinedRoomArea { get; set; } = "All Rooms Area";

        public string AccountingHeaderText { get; set; } = "Accounting";

        public string FlatManagementHeaderText { get; set; } = "Flat Management";
        public string FlatManagementInstructionText { get; set; }

        #endregion flat and room views, general stuff


        // payment views
        #region payment views
        public string IDF_Payments { get; set; } = "Payments";
        public string IDF_Quantity { get; set; } = "Quantity";
        public string IDF_AddPayment { get; set; } = "Add Payment(s)";
        public string IDF_DeletePayment { get; set; } = "Delete Payment";
        public string IDF_PaymentStartDate { get; set; } = "Start";
        public string IDF_PaymentEndDate { get; set; } = "End";
        public string IDF_Payment { get; set; } = "Payment";
        public string IDF_PaymentQuantity { get; set; } = "Quantity";
        public string IDF_PaymentTotal { get; set; } = "Total";
        #endregion payment views


        // rent views
        #region rent views

        public string IDF_Rent { get; set; } = "Rent";
        public string IDF_RentChange { get; set; } = "Rent Change(s)";

        public string IDF_RentUpdateData { get; set; } = "Rent Data"; 
        public string IDF_RentOptions { get; set; } = "Rent Options";

        public string IDF_RentShare { get; set; } = "Rent\nShare";
        public string IDF_AdvanceFixed { get; set; } = "Fixed\nAdvance";
        public string IDF_AdvanceHeating { get; set; } = "Heating\nAdvance";


        public string IDF_Fixed { get; set; } = "Fixed";
        public string IDF_Heating { get; set; } = "Heating";


        public string IDF_AdvanceTotal { get; set; } = "Total Costs";
        public string IDF_PriceTotal { get; set; } = "Total Price";
        public string IDF_BaseRentOnBilling { get; set; } = "add billing";
        public string IDF_AnnualBilling { get; set; } = "Annual Billing";
        
        public string IDF_Costs { get; set; } = "Other Costs"; 

        public string IDF_OtherCostsAdvance { get; set; } = "Other\nCosts";
        public string IDF_OtherCosts { get; set; } = "other costs details";

        public string IDF_OtherCostsList { get; set; } = "Additional Items";
       
        public string IDF_OtherCostsSum { get; set; } = "Sum";

        public string IDF_AnnualOther { get; set; } = "Other";

        public string IDF_AnnualComplete { get; set; } = "Complete";
        public string IDF_CompleteCosts { get; set; } = "Complete\nCosts";

        public string IDF_Credit { get; set; } = "Credit(s)";

        public string IDF_HasOtherCosts { get; set; } = "other costs";                
        public string IDF_NewOtherCosts { get; set; } = "setup other costs";
                
        public string IDF_FactorInCredit { get; set; } = "credits";
        public string IDF_FactorInPayments { get; set; } = "payments";


        public string IDF_NewCredit { get; set; } = "Setup Credit";
        public string IDF_LockData { get; set; } = "Lock Data";

        public string IDF_RentStartDate { get; set; } = "Begin";
        public string IDF_RentPerMonth { get; set; } = "Rent\nMonth";
        public string IDF_ExtraCostsPerMonth { get; set; }  = "Extra Costs\nMonth";
        public string IDF_TotalPricePerMonth { get; set; } = "Total Price\nMonth";

        public string IDF_RentPerYear { get; set; } = "Rent\nYear";
        public string IDF_ExtraCostsPerYear { get; set; } = "Extra Costs\nYear";
        public string IDF_TotalPricePerYear { get; set; } = "Total Price\nYear";

        public string IDF_SharedRent { get; set; } = "Shared Rent";
        public string IDF_Advance { get; set; } = "Advance";
        public string IDF_SharedAdvance { get; set; } = "Shared Advance";
        public string IDF_Price { get; set; } = "Price";

        public string IDF_Accounting { get; set; } = "Accounting";

        public string IDF_AnnualRent { get; set; } = "Rent";
        public string IDF_AnnualExtra { get; set; } = "Extra";
        public string IDF_AnnualFixed { get; set; } = "Fixed";
        public string IDF_AnnualHeating { get; set; } = "Heating";
        public string IDF_AnnualPrice { get; set; } = "Price";

        public string IDF_firstYear { get; set; } = "first year";
        public string IDF_perMonth { get; set; } = "per month";
        public string IDF_perRoom { get; set; } = "per room";
        public string IDF_perYear { get; set; } = "per year";

        public string RentManagementInstructionText { get; set; }

        #endregion rent views


        // settings view
        #region settings view

        public string IDF_SettingsTitleText { get; set; } = "SLCC Settings";
        public string IDF_Language { get; set; } = "Language";
        public string IDF_Culture { get; set; } = "Culture";
        public string IDF_FontSize { get; set; } = "Font Size";
        public string IDF_FontFamily { get; set; } = "Font Family";
        public string IDF_Foreground { get; set; } = "Text Color";
        public string IDF_Background { get; set; } = "Background";

        public string IDF_HeaderTextColor { get; set; } = "Header Color";
        public string IDF_SelectionColor { get; set; } = "Selection Color";
        public string IDF_ButtonCornerRadius { get; set; } = "Button Rounding";
        public string IDF_VisibilityFieldCornerRadius { get; set; } = "Field Rounding";

        public string SettingsHeaderText { get; set; } = "Settings";

        #endregion settings view

        #endregion properties


        // constructors
        #region constructors

        public LanguageResourceStrings()
        {
            _BillingManagementInstructionText();
            _FlatManagementInstructionText();
            _RentManagementInstructionText();
        }

        public LanguageResourceStrings(string selectedLanguage)
        {
            Language = selectedLanguage;

            _BillingManagementInstructionText();
            _FlatManagementInstructionText();
            _RentManagementInstructionText();
        }

        #endregion constructors


        //methods
        #region methods

        private string _BillingManagementInstructionText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
                "Billing Management\n" +
                "\n" +
                "-> click \"Add Billing\" to create new billing\n" +
                "-> select billing to view its data\n" +
                "-> click \"Delete\" to delete selected billing\n" +
                "-> click \"Show FinancialTransactionItems\" to display costs."
                );

            return stringBuilder.ToString();
        }

        private string _FlatManagementInstructionText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
                "Flat Management\n" +
                "\n" +
                "-> click \"New Flat\" to show flat setup window\n" +
                "\t setup flat, !!! address, area(s) and room counts can not be edited\n" +
                "\t -> click \"Proceed\" to create new flat\n" +
                "\t -> click \"Leave\" to return to flat management\n" +
                "-> select flat to view its most recent data\n" +
                "-> click \"Delete\" to delete selected flat\n" +
                "-> click \"Accounting\" to enter accounting for selected flat\n" +
                "-> click \"Settings\" to show settings window\n"
                );

            return stringBuilder.ToString();
        }

        private string _RentManagementInstructionText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
                "Rent Management\n" +
                "\n" +
                "-> click \"Add Rent\" to create new rent\n" +
                "-> select rent to view its data\n" +
                "-> click \"Delete\" to delete selected rent\n" +
                "-> specify billing in combobox if calculation\n" +
                "   should be based on consumption ratio and area ratio\n" +
                "-> click \"Show FinancialTransactionItems\" to display costs."
                );

            return stringBuilder.ToString();
        }

        #endregion methods


    }
}
// EOF