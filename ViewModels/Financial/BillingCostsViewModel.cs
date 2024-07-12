/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingCostsViewModel : BaseViewModel
 * 
 *  viewmodel for BillingCostsView
 *  
 *  displays the calculated results of 
 *  billing related data for the selected
 *  instance of FlatViewModel and the
 *  selected BillingViewModel instance
 */
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;


namespace SharedLivingCostCalculator.ViewModels.Financial
{
    internal class BillingCostsViewModel : BaseViewModel, ICostDisplay
    {

        // properties & fields
        #region properties

        public BillingViewModel BillingViewModel { get; }


        private readonly FlatViewModel _flatViewModel;


        private bool _ShowFlatCosts;
        public bool ShowFlatCosts
        {
            get { return _ShowFlatCosts; }
            set
            {
                _ShowFlatCosts = value;
                OnPropertyChanged(nameof(ShowFlatCosts));
            }
        }


        private bool _ShowRoomCosts;
        public bool ShowRoomCosts
        {
            get { return _ShowRoomCosts; }
            set
            {
                _ShowRoomCosts = value;
                OnPropertyChanged(nameof(ShowRoomCosts));
            }
        }

        #endregion properties


        // constructors
        #region constructors

        public BillingCostsViewModel(BillingViewModel billingViewModel, FlatViewModel flatViewModel)
        {
            BillingViewModel = billingViewModel;
            _flatViewModel = flatViewModel;

        }

        #endregion constructors


        // methods
        #region methods

        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }


        public IRoomCostsCarrier GetRoomCostsCarrier()
        {
            return BillingViewModel;
        }

        #endregion methods


    }
}
// EOF