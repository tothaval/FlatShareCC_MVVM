/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ConsumptionViewModel  : BaseViewModel
 * 
 *  viewmodel for ConsumptionView
 */
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.ComponentModel;

namespace SharedLivingCostCalculator.ViewModels.Contract
{
    public class ConsumptionViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private BillingViewModel _billingViewModel;
        public BillingViewModel BillingViewModel => _billingViewModel;


        public bool DataLock
        {
            get { return !_billingViewModel.HasDataLock; }
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
            _billingViewModel = billingViewModel;

            _billingViewModel.PropertyChanged += _billingViewModel_PropertyChanged;

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