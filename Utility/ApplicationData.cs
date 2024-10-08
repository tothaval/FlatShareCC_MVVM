﻿/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        public bool AnnualBilling_Shown { get; set; } = false;

        public bool FlatManagement_Shown { get; set; } = false;


        [XmlIgnore]
        private FlatManagementViewModel? _FlatManagementViewModel;


        public int FlatViewModelSelectedIndex { get; set; } = 0;


        public bool Manual_Shown { get; set; } = false;


        public bool Print_Shown { get; set; } = false;


        public bool Settings_Shown { get; set; } = false;

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

                AnnualBilling_Shown = _FlatManagementViewModel.ShowAnnualBilling;

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

                Print_Shown = _FlatManagementViewModel.ShowPrintView;

                Settings_Shown = _FlatManagementViewModel.ShowSettings;
            }
        }


        #endregion methods


    }
}
// EOF