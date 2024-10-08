﻿/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ConsumptionViewModel  : BaseViewModel
 * 
 *  viewmodel for ConsumptionView
 */
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.ComponentModel;

namespace SharedLivingCostCalculator.ViewModels.Contract
{
    public class ConsumptionViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private BillingViewModel _BillingViewModel;
        public BillingViewModel BillingViewModel => _BillingViewModel;


        public bool DataLock
        {
            get { return !_BillingViewModel.HasDataLock; }
        }


        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        #endregion event properties & fields


        // collections
        #region collections

        #endregion collections


        // constructors
        #region constructors

        public ConsumptionViewModel(BillingViewModel billingViewModel)
        {
            _BillingViewModel = billingViewModel;

            _BillingViewModel.PropertyChanged += _billingViewModel_PropertyChanged;

            SelectedIndex = 0;
        }

        #endregion constructors


        // events
        #region events

        private void _billingViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DataLock));
        }

        #endregion events


    }
}
// EOF