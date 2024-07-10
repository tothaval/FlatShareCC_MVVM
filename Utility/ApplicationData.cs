/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ApplicationData 
 * 
 *  serializable data model class 
 *  for storing ui state on exit
 */
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Utility
{
    [Serializable]
    [XmlRoot("ApplicationDataOnExit")]
    public class ApplicationData
    {

        // properties & fields
        #region properties & fields

        public bool Accounting_Shown { get; set; } = false;


        public bool FlatManagement_Shown { get; set; } = false;


        [XmlIgnore]
        private FlatManagementViewModel? _FlatManagementViewModel;


        public bool FlatSetup_Shown { get; set; } = false;


        public int FlatViewModelSelectedIndex { get; set; } = 0;


        public bool Manual_Shown { get; set; } = false;


        public bool RoomSetup_Shown { get; set; } = false;
        

        public bool Settings_Shown { get; set; } = false;


        public bool ShowCosts_Shown { get; set; } = false;

        #endregion properties & fields


        // constructors
        #region constructors

        public ApplicationData()
        {
                
        }


        public ApplicationData(BaseViewModel baseViewModel)
        {
            if (baseViewModel.GetType() == typeof(FlatManagementViewModel))
            {
                _FlatManagementViewModel = baseViewModel as FlatManagementViewModel;

                SetData();
            }
        }

        #endregion constructors


        // methods
        #region methods

        private void SetData()
        {
            if (_FlatManagementViewModel != null)
            {
                Accounting_Shown = _FlatManagementViewModel.ShowAccounting;

                FlatSetup_Shown = _FlatManagementViewModel.ShowFlatSetup;

                FlatManagement_Shown = _FlatManagementViewModel.ShowFlatManagement;

                if (_FlatManagementViewModel.FlatCollection.Count > 0)
                {
                    FlatViewModelSelectedIndex = _FlatManagementViewModel.FlatCollection.IndexOf(
                        _FlatManagementViewModel.SelectedItem
                        );
                }
                else
                {
                    FlatViewModelSelectedIndex = -1;
                }

                Manual_Shown = _FlatManagementViewModel.ShowManual;

                RoomSetup_Shown = _FlatManagementViewModel.ShowRoomSetup;

                Settings_Shown = _FlatManagementViewModel.ShowSettings;

                ShowCosts_Shown = _FlatManagementViewModel.ShowCosts;

            }
        }


        #endregion methods


    }
}
// EOF