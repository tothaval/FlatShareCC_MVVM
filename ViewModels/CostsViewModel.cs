/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  CostsViewModel  : BaseViewModel
 * 
 *  viewmodel for CostView
 *  
 *  shows either a RentCostsViewModel or BillingCostsViewModel
 *  depending on constructor
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Security;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class CostsViewModel : BaseViewModel
    {

        private readonly AccountingViewModel _AccountingViewModel;
        public AccountingViewModel Accounting => _AccountingViewModel;


        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;


        private BillingViewModel? _billingViewModel;


        private RentViewModel? _rentViewModel;


        private BaseViewModel _ActiveViewModel;
        public BaseViewModel ActiveViewModel
        {
            get { return _ActiveViewModel; }
            set
            {
                _ActiveViewModel = value;

                OnPropertyChanged(nameof(ActiveViewModel));
            }
        }


        private bool _BillingSelected;
        public bool BillingSelected
        {
            get { return _BillingSelected; }
            set
            {
                _BillingSelected = value;

                if (_BillingSelected && _billingViewModel != null && _flatViewModel != null)
                {
                    ActiveViewModel = new BillingCostsViewModel(_billingViewModel, _flatViewModel);
                }
                OnPropertyChanged(nameof(BillingSelected));
            }
        }


        public bool HasBilling => _billingViewModel != null;


        private bool _RentSelected;
        public bool RentSelected
        {
            get { return _RentSelected; }
            set
            {
                _RentSelected = value;

                if (_RentSelected && _rentViewModel != null && _flatViewModel != null)
                {
                    ActiveViewModel = new RentCostsViewModel(_rentViewModel, _flatViewModel);
                }
                OnPropertyChanged(nameof(RentSelected));
            }
        }


        public CostsViewModel(AccountingViewModel accountingViewModel)
        {
            _AccountingViewModel = accountingViewModel;

            _AccountingViewModel.AccountingChanged += _AccountingViewModel_AccountingChanged;

            RentSelected = true;

            Update();
        }


        private void Update()
        {
            if (_AccountingViewModel.FlatViewModel != null)
            {
                _flatViewModel = _AccountingViewModel.FlatViewModel;
            }

            if (_AccountingViewModel.Rents.SelectedValue != null)
            {
                _rentViewModel = _AccountingViewModel.Rents.SelectedValue;
                ActiveViewModel = new RentCostsViewModel(_rentViewModel, _flatViewModel);
            }

            if (_rentViewModel != null && _rentViewModel.BillingViewModel != null)
            {
                _billingViewModel = _rentViewModel.BillingViewModel;
            }

            OnPropertyChanged(nameof(FlatViewModel));
            OnPropertyChanged(nameof(ActiveViewModel));
            OnPropertyChanged(nameof(HasBilling));
        }


        private void _AccountingViewModel_AccountingChanged(object? sender, EventArgs e)
        {
            Update();
        }


    }
}
// EOF