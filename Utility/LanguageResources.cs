/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  LanguageResources 
 * 
 *  helper class of the language infrastructure
 *  which assignes the members of a LanguageResourceStrings instance
 *  to the application resources 
 */
using System.Collections.ObjectModel;
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


        // properties & fields
        #region properties

        private string _SelectedLanguage;

        #endregion properties


        // constructors
        #region constructors
        
        public LanguageResources()
        {
                
        }


        public LanguageResources(string language)
        {
            _SelectedLanguage = language;

            LoadLanguageResource();

        }

        #endregion constructors


        // methods
        #region methods

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

                if (_SelectedLanguage.Equals(file_short) && !file_short.StartsWith("__"))
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

        public ObservableCollection<string> LoadLanguages()
        {
            ObservableCollection<string> languages = new ObservableCollection<string>();

            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\language\\";
            string filter = "*.xml";

            List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();

            foreach (string file in files)
            {
                string pathlessFile = Path.GetFileName(file);

                string file_short = pathlessFile.Replace(".xml", "");

                if (!file_short.StartsWith("__"))
                {
                   languages.Add(file_short);
                }
            }

            return languages;
        }


        public void SetResources(LanguageResourceStrings languageResource)
        {

            // billing views
            #region billing views

            Application.Current.Resources["IDF_AnnualBillings"] = languageResource.IDF_AnnualBillings;

            Application.Current.Resources["IDF_Billing"] = languageResource.IDF_Billing;
            Application.Current.Resources["IDF_BillingDate"] = languageResource.IDF_BillingDate;            
            Application.Current.Resources["IDF_BillingPeriodData"] = languageResource.IDF_BillingPeriodData;
            Application.Current.Resources["IDF_BillingPeriodOptions"] = languageResource.IDF_BillingPeriodOptions;
            Application.Current.Resources["IDF_Consumption"] = languageResource.IDF_Consumption;
            Application.Current.Resources["IDF_ConsumptionShare"] = languageResource.IDF_ConsumptionShare;
            Application.Current.Resources["IDF_CombinedConsumption"] = languageResource.IDF_CombinedConsumption;
            Application.Current.Resources["IDF_ConsumptionPercentage"] = languageResource.IDF_ConsumptionPercentage;
                       
            Application.Current.Resources["IDF_ExtraCosts"] = languageResource.IDF_ExtraCosts;
            Application.Current.Resources["IDF_FixedCosts"] = languageResource.IDF_FixedCosts;
            Application.Current.Resources["IDF_HeatingCosts"] = languageResource.IDF_HeatingCosts;
            Application.Current.Resources["IDF_TotalCosts"] = languageResource.IDF_TotalCosts;
            Application.Current.Resources["IDF_Balance"] = languageResource.IDF_Balance;

            
            Application.Current.Resources["IDF_OtherCostsData"] = languageResource.IDF_OtherCostsData;
            Application.Current.Resources["IDF_RoomCostShares"] = languageResource.IDF_RoomCostShares;
            Application.Current.Resources["IDF_RoomData"] = languageResource.IDF_RoomData;
             
            Application.Current.Resources["IDF_RoomHeatingUnits"] = languageResource.IDF_RoomHeatingUnits;
            Application.Current.Resources["IDF_CombinedRoomHeatingUnits"] = languageResource.IDF_CombinedRoomHeatingUnits;
            Application.Current.Resources["IDF_Percentage"] = languageResource.IDF_Percentage;
            Application.Current.Resources["IDF_TotalConsumption"] = languageResource.IDF_TotalConsumption;
            Application.Current.Resources["IDF_SharedConsumption"] = languageResource.IDF_SharedConsumption;
            Application.Current.Resources["IDF_TotalRooms"] = languageResource.IDF_TotalRooms;
            Application.Current.Resources["IDF_Year"] = languageResource.IDF_Year;

            #endregion billing views


            // buttons and commands
            #region buttons and commands

            Application.Current.Resources["IDF_Back"] = languageResource.IDF_Back;
            Application.Current.Resources["IDF_NewBilling"] = languageResource.IDF_NewBilling;
            Application.Current.Resources["IDF_DeleteBilling"] = languageResource.IDF_DeleteBilling;            
            Application.Current.Resources["IDF_PrintView"] = languageResource.IDF_PrintView;
            
            Application.Current.Resources["IDF_Other"] = languageResource.IDF_Other;
            Application.Current.Resources["IDF_Tenants"] = languageResource.IDF_Tenants;
            
            Application.Current.Resources["IDF_ShowCosts"] = languageResource.IDF_ShowCosts;
            Application.Current.Resources["IDF_NewRent"] = languageResource.IDF_NewRent;
            Application.Current.Resources["IDF_NewRentChange"] = languageResource.IDF_NewRentChange;
            Application.Current.Resources["IDF_Raise"] = languageResource.IDF_Raise;           
            Application.Current.Resources["IDF_DeleteRent"] = languageResource.IDF_DeleteRent;
            Application.Current.Resources["IDF_DeleteRentChange"] = languageResource.IDF_DeleteRentChange;

            Application.Current.Resources["IDF_NewFlat"] = languageResource.IDF_NewFlat;
            Application.Current.Resources["IDF_EditFlat"] = languageResource.IDF_EditFlat;
            Application.Current.Resources["IDF_DeleteFlat"] = languageResource.IDF_DeleteFlat;
            Application.Current.Resources["IDF_Settings"] = languageResource.IDF_Settings;
            Application.Current.Resources["IDF_Manual"] = languageResource.IDF_Manual;
            Application.Current.Resources["IDF_FlatSetup"] = languageResource.IDF_FlatSetup;
            Application.Current.Resources["IDF_RoomSetup"] = languageResource.IDF_RoomSetup;
            Application.Current.Resources["IDF_FlatCosts"] = languageResource.IDF_FlatCosts;
            Application.Current.Resources["IDF_RoomCosts"] = languageResource.IDF_RoomCosts;

            #endregion buttons and commands


            // flat and room views, general stuff
            #region flat and room views, general stuff
            
            Application.Current.Resources["IDF_ContractStart"] = languageResource.IDF_ContractStart;
            Application.Current.Resources["IDF_StartDate"] = languageResource.IDF_StartDate;
            Application.Current.Resources["IDF_EndDate"] = languageResource.IDF_EndDate;

            Application.Current.Resources["IDF_ID"] = languageResource.IDF_ID;
            Application.Current.Resources["IDF_RoomName"] = languageResource.IDF_RoomName;
            Application.Current.Resources["IDF_RoomArea"] = languageResource.IDF_RoomArea;

            Application.Current.Resources["IDF_MainWindowTitle"] = languageResource.IDF_MainWindowTitle;

            Application.Current.Resources["IDF_Address"] = languageResource.IDF_Address;
            Application.Current.Resources["IDF_FlatArea"] = languageResource.IDF_FlatArea;
            Application.Current.Resources["IDF_FlatData"] = languageResource.IDF_FlatData;
            Application.Current.Resources["IDF_Details"] = languageResource.IDF_Details;
            Application.Current.Resources["IDF_Rooms"] = languageResource.IDF_Rooms;            

            Application.Current.Resources["IDF_RoomTenant"] = languageResource.IDF_RoomTenant;
            Application.Current.Resources["IDF_TenantSetup"] = languageResource.IDF_TenantSetup;
            Application.Current.Resources["IDF_MovingIn"] = languageResource.IDF_MovingIn;
            Application.Current.Resources["IDF_MovingOut"] = languageResource.IDF_MovingOut;
            Application.Current.Resources["IDF_TenantIsActive"] = languageResource.IDF_TenantIsActive;
            Application.Current.Resources["IDF_TenantManagement"] = languageResource.IDF_TenantManagement;            
            Application.Current.Resources["IDF_ActiveTenantCount"] = languageResource.IDF_ActiveTenantCount;


            Application.Current.Resources["IDF_RentedAreaShare"] = languageResource.IDF_RentedAreaShare;
            Application.Current.Resources["IDF_SharedFlatArea"] = languageResource.IDF_SharedFlatArea; 
            Application.Current.Resources["IDF_CombinedRoomArea"] = languageResource.IDF_CombinedRoomArea;

            Application.Current.Resources["AccountingHeaderText"] = languageResource.AccountingHeaderText;

            Application.Current.Resources["FlatManagementHeaderText"] = languageResource.FlatManagementHeaderText;

            #endregion flat and room views, general stuff


            // payment views
            #region payment views

            Application.Current.Resources["IDF_Payments"] = languageResource.IDF_Payments;
            Application.Current.Resources["IDF_Quantity"] = languageResource.IDF_Quantity;
            Application.Current.Resources["IDF_AddPayment"] = languageResource.IDF_AddPayment;
            Application.Current.Resources["IDF_DeletePayment"] = languageResource.IDF_DeletePayment;
            Application.Current.Resources["IDF_PaymentStartDate"] = languageResource.IDF_PaymentStartDate;
            Application.Current.Resources["IDF_PaymentEndDate"] = languageResource.IDF_PaymentEndDate;
            Application.Current.Resources["IDF_Payment"] = languageResource.IDF_Payment;
            Application.Current.Resources["IDF_PaymentQuantity"] = languageResource.IDF_PaymentQuantity;
            Application.Current.Resources["IDF_PaymentTotal"] = languageResource.IDF_PaymentTotal;

            #endregion payment views


            // rent views
            #region rent views

            Application.Current.Resources["IDF_Rent"] = languageResource.IDF_Rent;
            Application.Current.Resources["IDF_RentChange"] = languageResource.IDF_RentChange;

            Application.Current.Resources["IDF_RentUpdateData"] = languageResource.IDF_RentUpdateData;
            Application.Current.Resources["IDF_RentOptions"] = languageResource.IDF_RentOptions;

            Application.Current.Resources["IDF_RentShare"] = languageResource.IDF_RentShare;
            Application.Current.Resources["IDF_Advance"] = languageResource.IDF_Advance;

            Application.Current.Resources["IDF_Fixed"] = languageResource.IDF_Fixed;
            Application.Current.Resources["IDF_Heating"] = languageResource.IDF_Heating;             


            Application.Current.Resources["IDF_PriceTotal"] = languageResource.IDF_PriceTotal;
            Application.Current.Resources["IDF_BaseRentOnBilling"] = languageResource.IDF_BaseRentOnBilling;
            Application.Current.Resources["IDF_AnnualBilling"] = languageResource.IDF_AnnualBilling;

            Application.Current.Resources["IDF_HasOtherCosts"] = languageResource.IDF_HasOtherCosts;
            Application.Current.Resources["IDF_NewOtherCosts"] = languageResource.IDF_NewOtherCosts;
            Application.Current.Resources["IDF_Costs"] = languageResource.IDF_Costs; 
            Application.Current.Resources["IDF_OtherCosts"] = languageResource.IDF_OtherCosts;
            Application.Current.Resources["IDF_OtherCostsList"] = languageResource.IDF_OtherCostsList;
            Application.Current.Resources["IDF_OtherCostsSum"] = languageResource.IDF_OtherCostsSum;
            Application.Current.Resources["IDF_OtherCostsAdvance"] = languageResource.IDF_OtherCostsAdvance;
            
            Application.Current.Resources["IDF_CompleteCosts"] = languageResource.IDF_CompleteCosts;

            Application.Current.Resources["IDF_Credit"] = languageResource.IDF_Credit;
            Application.Current.Resources["IDF_FactorInCredit"] = languageResource.IDF_FactorInCredit;
            Application.Current.Resources["IDF_FactorInPayments"] = languageResource.IDF_FactorInPayments;

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

            Application.Current.Resources["IDF_perYear"] = languageResource.IDF_perYear;         
            Application.Current.Resources["IDF_perMonth"] = languageResource.IDF_perMonth;
            Application.Current.Resources["IDF_perRoom"] = languageResource.IDF_perRoom;
            
            #endregion rent views


            // setting views
            #region setting views

            Application.Current.Resources["IDF_Background"] = languageResource.IDF_Background; 

            Application.Current.Resources["IDF_ButtonBackground"] = languageResource.IDF_ButtonBackground;

            Application.Current.Resources["IDF_ButtonCornerRadius"] = languageResource.IDF_ButtonCornerRadius;

            Application.Current.Resources["IDF_Culture"] = languageResource.IDF_Culture;

            Application.Current.Resources["IDF_FontFamily"] = languageResource.IDF_FontFamily;

            Application.Current.Resources["IDF_FontSize"] = languageResource.IDF_FontSize;

            Application.Current.Resources["IDF_Foreground"] = languageResource.IDF_Foreground;

            Application.Current.Resources["IDF_HeaderTextColor"] = languageResource.IDF_HeaderTextColor;

            Application.Current.Resources["IDF_Language"] = languageResource.IDF_Language;

            Application.Current.Resources["IDF_SelectionColor"] = languageResource.IDF_SelectionColor;

            Application.Current.Resources["SettingsHeaderText"] = languageResource.SettingsHeaderText;

            Application.Current.Resources["IDF_SettingsTitleText"] = languageResource.IDF_SettingsTitleText;

            Application.Current.Resources["IDF_VisibilityFieldCornerRadius"] = languageResource.IDF_VisibilityFieldCornerRadius;

        #endregion setting views


    }
        #endregion methods


    }
}
// EOF