/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
using SharedLivingCostCalculator.Enums;

namespace SharedLivingCostCalculator.Utility
{
    [Serializable]
    [XmlRoot("Language")]
    public class LanguageResourceStrings
    {
        public string Language { get; set; } = "";

        // flat and room views, general stuff
        public string IDF_StartDate { get; set; } = "Begin";
        public string IDF_EndDate { get; set; } = "End";

        public string IDF_ID { get; set; } = "ID";
        public string IDF_RoomName { get; set; } = "Room";
        public string IDF_RoomArea { get; set; } = "Area";

        public string IDF_MainWindowTitle { get; set; } = "Shared Living Cost Calculator (SLCC)";
        public string IDF_FlatSetupTitleText { get; set; } = "SLCC FlatSetup";
        public string FlatSetupHeaderText { get; set; } = "Flat Setup";

        public string IDF_Address { get; set; } = "Address";
        public string IDF_FlatArea { get; set; } = "Flat Area";
        public string IDF_Details { get; set; } = "Details";
        public string IDF_Rooms { get; set; } = "Rooms";

        public string IDF_RoomTenant { get; set; } = "Tenant";

        public string IDF_SharedFlatArea { get; set; } = "Shared Area"; 
        public string IDF_CombinedRoomArea { get; set; } = "All Rooms Area";

        public string AccountingHeaderText { get; set; } = "Accounting";

        public string FlatManagementHeaderText { get; set; } = "Flat Management";
        public string FlatManagementInstructionText { get; set; }

        // settings view
        public string IDF_SettingsTitleText { get; set; } = "SLCC Settings";
        public string IDF_Language { get; set; } = "Language";
        public string IDF_Country { get; set; } = "Country";
        public string IDF_FontSize { get; set; } = "Font Size";
        public string IDF_FontFamily { get; set; } = "Font Family";
        public string IDF_Foreground { get; set; } = "Foreground";
        public string IDF_Background { get; set; } = "Background";

        public string SettingsHeaderText { get; set; } = "Settings";
                   
        // rent views
        public string IDF_Rent { get; set; } = "Rent";
        public string IDF_RentChange { get; set; } = "Rent Change(s)";

        public string IDF_RentUpdateData { get; set; } = "Rent Data"; 
        public string IDF_RentOptions { get; set; } = "Rent Options";

        public string IDF_RentShare { get; set; } = "Rent Share\nMonth";
        public string IDF_AdvanceFixed { get; set; } = "Fixed Costs\nAdvance";
        public string IDF_AdvanceHeating { get; set; } = "Heating Costs\nAdvance";
        public string IDF_AdvanceTotal { get; set; } = "Total Costs\nAdvance";
        public string IDF_PriceTotal { get; set; } = "Total Price\nMonth";
        public string IDF_BaseRentOnBilling { get; set; } = "factor in billing";
        public string IDF_AnnualBilling { get; set; } = "Annual\nBilling";
        
        public string IDF_OtherCosts { get; set; } = "Other Costs\nMonth";

        public string IDF_AnnualOther { get; set; } = "Other Costs\nAnnual";
        public string IDF_AnnualComplete { get; set; } = "Complete Costs\nAnnual";
        public string IDF_CompleteCosts { get; set; } = "Complete Costs\nMonth";


        public string IDF_Credit { get; set; } = "credit(s)";

        public string IDF_HasOtherCosts { get; set; } = "factor in other costs";
        
        
        public string IDF_NewOtherCosts { get; set; } = "setup other costs";
        
        
        public string IDF_FactorInCredit { get; set; } = "factor in a credit";
        public string IDF_FactorInPayments { get; set; } = "factor in payments";


        public string IDF_NewCredit { get; set; } = "setup credit";
        public string IDF_LockData { get; set; } = "lock data";

        public string IDF_RentStartDate { get; set; } = "begin\n\n";
        public string IDF_RentPerMonth { get; set; } = "rent\nmonth";
        public string IDF_ExtraCostsPerMonth { get; set; }  = "extra costs\nmonth";
        public string IDF_TotalPricePerMonth { get; set; } = "total price\nmonth";

        public string IDF_RentPerYear { get; set; } = "rent\nyear";
        public string IDF_ExtraCostsPerYear { get; set; } = "extra costs\nyear";
        public string IDF_TotalPricePerYear { get; set; } = "total price\nyear";

        public string IDF_SharedRent { get; set; } = "shared rent";
        public string IDF_Advance { get; set; } = "advance";
        public string IDF_SharedAdvance { get; set; } = "shared advance";
        public string IDF_Price { get; set; } = "price";

        public string IDF_Accounting { get; set; } = "accounting";

        public string IDF_AnnualRent { get; set; } = "annual rent";
        public string IDF_AnnualExtra { get; set; } = "annual extra";
        public string IDF_AnnualFixed { get; set; } = "annual fixed";
        public string IDF_AnnualHeating { get; set; } = "annual heating";
        public string IDF_AnnualPrice { get; set; } = "annual price";

        public string IDF_perYear { get; set; } = "per year";
        public string IDF_perMonth { get; set; } = "per month";

        public string RentManagementInstructionText { get; set; }

        
        // billing views
        public string IDF_Billing { get; set; } = "Billing";
        public string IDF_BillingPeriodData { get; set; } = "Billing Data"; 
        public string IDF_BillingPeriodOptions { get; set; } = "Billing Options";
        public string IDF_Consumption { get; set; } = "Consumption";

        public string IDF_ExtraCosts { get; set; } = "Extra\nCosts";
        public string IDF_FixedCosts { get; set; } = "Fixed\nCosts";
        public string IDF_HeatingCosts { get; set; }  = "Heating\nCosts";
        public string IDF_TotalCosts { get; set; } = "Total\nCosts";
        public string IDF_Balance { get; set; } = "Balance";

        public string IDF_RoomHeatingUnits { get; set; } = "Heating Units";
        public string IDF_CombinedRoomHeatingUnits { get; set; } = "all rooms\nheating units";
        public string IDF_Percentage { get; set; } = "%";
        public string IDF_TotalConsumption { get; set; } = "total consumption";
        public string IDF_TotalRooms { get; set; } = "rooms consumption";

        public string BillingManagementInstructionText { get; set; }


        // payment views
        public string IDF_Payments { get; set; } = "Payments";
        public string IDF_Quantity { get; set; } = "Quantity";
        public string IDF_AddPayment { get; set; } = "Add Payment(s)";
        public string IDF_DeletePayment { get; set; } = "Delete Payment";
        public string IDF_PaymentStartDate { get; set; } = "start";
        public string IDF_PaymentEndDate { get; set; } = "end";
        public string IDF_Payment { get; set; } = "payment";
        public string IDF_PaymentQuantity { get; set; } = "payment\nquantity";
        public string IDF_PaymentTotal { get; set; } = "total";


        // buttons and commands
        public string IDF_Back { get; set; } = "Back";
        public string IDF_NewBilling { get; set; } = "New Billing";
        public string IDF_DeleteBilling { get; set; } = "Delete";
        public string IDF_ShowCosts { get; set; } = "Show Costs";
        public string IDF_NewRent { get; set; } = "New Rent";
        public string IDF_NewRentChange { get; set; } = "New Rent Change";
        public string IDF_DeleteRent { get; set; } = "Delete Rent";
        public string IDF_DeleteRentChange { get; set; } = "Delete Rent Change(s)";


        public string IDF_NewFlat { get; set; } = "New Flat";
        public string IDF_EditFlat { get; set; } = "Edit Flat";
        public string IDF_DeleteFlat { get; set; } = "Delete Flat";
        public string IDF_FlatSetup { get; set; } = "Flat Setup";
        public string IDF_RoomSetup { get; set; } = "Room Setup";
        public string IDF_Manual { get; set; } = "Manual";
        public string IDF_Settings { get; set; } = "Settings"; 



        public LanguageResourceStrings()
        {
            _BillingManagementInstructionText();
            _FlatManagementInstructionText();
            _RentManagementInstructionText();
        }

        public LanguageResourceStrings(SupportedLanguages supportedLanguages)
        {
            Language = supportedLanguages.ToString();

            _BillingManagementInstructionText();
            _FlatManagementInstructionText();
            _RentManagementInstructionText();
        }

        private string _BillingManagementInstructionText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
                "Billing Management\n" +
                "\n" +
                "-> click \"Add Billing\" to create new billing\n" +
                "-> select billing to view its data\n" +
                "-> click \"Delete\" to delete selected billing\n" +
                "-> click \"Show Costs\" to display costs."
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
                "-> click \"Show Costs\" to display costs."
                );

            return stringBuilder.ToString();
        }
    }
}
// EOF