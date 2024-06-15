/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  LanguageResources 
 * 
 *  helper class of the language infrastructure
 *  which assignes the members of a LanguageResourceStrings instance
 *  to the application resources 
 */
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Utility
{
    public class LanguageResources
    {

        // to do
        // change this into a list/collection and use it
        // as itemsource for the language combobox.
        // this way languages could be added very easy
        // by adding an apropriate file into the language
        // folder.
        private SupportedLanguages _SupportedLanguages;


        public LanguageResources(string language)
        {
            _SupportedLanguages = (SupportedLanguages)System.Enum.Parse(typeof(SupportedLanguages), language);

            LoadLanguageResource();

        }


        public void ChangeResources()
        {

        }


        private void LoadLanguageResource()
        {
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\language\\";
            string filter = "*.xml";

            List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();

            foreach (string file in files)
            {
                string pathlessFile = Path.GetFileName(file);

                string file_short = pathlessFile.Replace(".xml", "");

                if (_SupportedLanguages.ToString().Equals(file_short) && !file_short.StartsWith("__"))
                {
                    var xmlSerializer = new XmlSerializer(typeof(LanguageResourceStrings));

                    using (var writer = new StreamReader(file))
                    {
                        try
                        {
                            var member = (LanguageResourceStrings)xmlSerializer.Deserialize(writer);

                            SetResources(member);

                            break;
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }


        public void SetResources(LanguageResourceStrings languageResource)
        {
            // flat and room views, general stuff
            Application.Current.Resources["IDF_StartDate"] = languageResource.IDF_StartDate;
            Application.Current.Resources["IDF_EndDate"] = languageResource.IDF_EndDate;

            Application.Current.Resources["IDF_ID"] = languageResource.IDF_ID;
            Application.Current.Resources["IDF_RoomName"] = languageResource.IDF_RoomName;
            Application.Current.Resources["IDF_RoomArea"] = languageResource.IDF_RoomArea;

            Application.Current.Resources["IDF_MainWindowTitle"] = languageResource.IDF_MainWindowTitle;
            Application.Current.Resources["IDF_FlatSetupTitleText"] = languageResource.IDF_FlatSetupTitleText;
            Application.Current.Resources["FlatSetupHeaderText"] = languageResource.FlatSetupHeaderText;

            Application.Current.Resources["IDF_Address"] = languageResource.IDF_Address;
            Application.Current.Resources["IDF_FlatArea"] = languageResource.IDF_FlatArea;
            Application.Current.Resources["IDF_Details"] = languageResource.IDF_Details;
            Application.Current.Resources["IDF_Rooms"] = languageResource.IDF_Rooms;

            Application.Current.Resources["IDF_RoomTenant"] = languageResource.IDF_RoomTenant;

            Application.Current.Resources["IDF_SharedFlatArea"] = languageResource.IDF_SharedFlatArea;

            Application.Current.Resources["AccountingHeaderText"] = languageResource.AccountingHeaderText;

            Application.Current.Resources["FlatManagementHeaderText"] = languageResource.FlatManagementHeaderText;
            Application.Current.Resources["FlatManagementInstructionText"] = languageResource.FlatManagementInstructionText;


            // settings view
            Application.Current.Resources["IDF_SettingsTitleText"] = languageResource.IDF_SettingsTitleText;
            Application.Current.Resources["IDF_Language"] = languageResource.IDF_Language;
            Application.Current.Resources["IDF_Country"] = languageResource.IDF_Country;
            Application.Current.Resources["IDF_FontSize"] = languageResource.IDF_FontSize;
            Application.Current.Resources["IDF_FontFamily"] = languageResource.IDF_FontFamily;
            Application.Current.Resources["IDF_Foreground"] = languageResource.IDF_Foreground;
            Application.Current.Resources["IDF_Background"] = languageResource.IDF_Background;

            Application.Current.Resources["SettingsHeaderText"] = languageResource.SettingsHeaderText;


            // rent views
            Application.Current.Resources["IDF_Rent"] = languageResource.IDF_Rent;

            Application.Current.Resources["IDF_RentUpdateData"] = languageResource.IDF_RentUpdateData;

            Application.Current.Resources["IDF_RentShare"] = languageResource.IDF_RentShare;
            Application.Current.Resources["IDF_AdvanceFixed"] = languageResource.IDF_AdvanceFixed;
            Application.Current.Resources["IDF_AdvanceHeating"] = languageResource.IDF_AdvanceHeating;
            Application.Current.Resources["IDF_AdvanceTotal"] = languageResource.IDF_AdvanceTotal;
            Application.Current.Resources["IDF_PriceTotal"] = languageResource.IDF_PriceTotal;
            Application.Current.Resources["IDF_BaseRentOnBilling"] = languageResource.IDF_BaseRentOnBilling;

            Application.Current.Resources["IDF_HasOtherCosts"] = languageResource.IDF_HasOtherCosts;
            Application.Current.Resources["IDF_NewOtherCosts"] = languageResource.IDF_NewOtherCosts;
            Application.Current.Resources["IDF_CreditReceived"] = languageResource.IDF_CreditReceived;
            Application.Current.Resources["IDF_NewCredit"] = languageResource.IDF_NewCredit;

            Application.Current.Resources["IDF_LockData"] = languageResource.IDF_LockData;

            Application.Current.Resources["IDF_RentStartDate"] = languageResource.IDF_RentStartDate;
            Application.Current.Resources["IDF_RentPerMonth"] = languageResource.IDF_RentPerMonth;
            Application.Current.Resources["IDF_ExtraCostsPerMonth"] = languageResource.IDF_ExtraCostsPerMonth;
            Application.Current.Resources["IDF_TotalPricePerMonth"] = languageResource.IDF_TotalPricePerMonth;

            Application.Current.Resources["IDF_RentPerYear"] = languageResource.IDF_RentPerYear;
            Application.Current.Resources["IDF_ExtraCostsPerYear"] = languageResource.IDF_ExtraCostsPerYear;
            Application.Current.Resources["IDF_TotalPricePerYear"] = languageResource.IDF_TotalPricePerYear;

            Application.Current.Resources["IDF_SharedRent"] = languageResource.IDF_SharedRent;
            Application.Current.Resources["IDF_Advance"] = languageResource.IDF_Advance;
            Application.Current.Resources["IDF_SharedAdvance"] = languageResource.IDF_SharedAdvance;
            Application.Current.Resources["IDF_Price"] = languageResource.IDF_Price;

            Application.Current.Resources["IDF_Accounting"] = languageResource.IDF_Accounting;

            Application.Current.Resources["IDF_AnnualRent"] = languageResource.IDF_AnnualRent;
            Application.Current.Resources["IDF_AnnualExtra"] = languageResource.IDF_AnnualExtra;
            Application.Current.Resources["IDF_AnnualFixed"] = languageResource.IDF_AnnualFixed;
            Application.Current.Resources["IDF_AnnualHeating"] = languageResource.IDF_AnnualHeating;
            Application.Current.Resources["IDF_AnnualPrice"] = languageResource.IDF_AnnualPrice;

            Application.Current.Resources["IDF_perYear"] = languageResource.IDF_perYear;
            Application.Current.Resources["IDF_perMonth"] = languageResource.IDF_perMonth;

            Application.Current.Resources["RentManagementInstructionText"] = languageResource.RentManagementInstructionText;


            // billing views
            Application.Current.Resources["IDF_Billing"] = languageResource.IDF_Billing;
            Application.Current.Resources["IDF_BillingPeriodData"] = languageResource.IDF_BillingPeriodData;
            Application.Current.Resources["IDF_Consumption"] = languageResource.IDF_Consumption;

            Application.Current.Resources["IDF_ExtraCosts"] = languageResource.IDF_ExtraCosts;
            Application.Current.Resources["IDF_FixedCosts"] = languageResource.IDF_FixedCosts;
            Application.Current.Resources["IDF_HeatingCosts"] = languageResource.IDF_HeatingCosts;
            Application.Current.Resources["IDF_TotalCosts"] = languageResource.IDF_TotalCosts;
            Application.Current.Resources["IDF_Balance"] = languageResource.IDF_Balance;

            Application.Current.Resources["IDF_RoomHeatingUnits"] = languageResource.IDF_RoomHeatingUnits;
            Application.Current.Resources["IDF_CombinedRoomHeatingUnits"] = languageResource.IDF_CombinedRoomHeatingUnits;
            Application.Current.Resources["IDF_Percentage"] = languageResource.IDF_Percentage;
            Application.Current.Resources["IDF_TotalConsumption"] = languageResource.IDF_TotalConsumption;
            Application.Current.Resources["IDF_TotalRooms"] = languageResource.IDF_TotalRooms;

            Application.Current.Resources["BillingManagementInstructionText"] = languageResource.BillingManagementInstructionText;

            // payment views
            Application.Current.Resources["IDF_Payments"] = languageResource.IDF_Payments;
            Application.Current.Resources["IDF_Quantity"] = languageResource.IDF_Quantity;
            Application.Current.Resources["IDF_AddPayment"] = languageResource.IDF_AddPayment;
            Application.Current.Resources["IDF_DeletePayment"] = languageResource.IDF_DeletePayment;
            Application.Current.Resources["IDF_PaymentStartDate"] = languageResource.IDF_PaymentStartDate;
            Application.Current.Resources["IDF_PaymentEndDate"] = languageResource.IDF_PaymentEndDate;
            Application.Current.Resources["IDF_Payment"] = languageResource.IDF_Payment;
            Application.Current.Resources["IDF_PaymentQuantity"] = languageResource.IDF_PaymentQuantity;
            Application.Current.Resources["IDF_PaymentTotal"] = languageResource.IDF_PaymentTotal;

            // buttons and commands
            Application.Current.Resources["IDF_Back"] = languageResource.IDF_Back;
            Application.Current.Resources["IDF_NewBilling"] = languageResource.IDF_NewBilling;
            Application.Current.Resources["IDF_DeleteBilling"] = languageResource.IDF_DeleteBilling;
            Application.Current.Resources["IDF_ShowCosts"] = languageResource.IDF_ShowCosts;
            Application.Current.Resources["IDF_NewRent"] = languageResource.IDF_NewRent;
            Application.Current.Resources["IDF_DeleteRent"] = languageResource.IDF_DeleteRent;

            Application.Current.Resources["IDF_NewFlat"] = languageResource.IDF_NewFlat;
            Application.Current.Resources["IDF_EditFlat"] = languageResource.IDF_EditFlat;
            Application.Current.Resources["IDF_DeleteFlat"] = languageResource.IDF_DeleteFlat;
            Application.Current.Resources["IDF_Settings"] = languageResource.IDF_Settings;


        }
    }
}
// EOF