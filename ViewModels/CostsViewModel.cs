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

        // properties & fields
        #region properties & fields

        private readonly AccountingViewModel _AccountingViewModel;
        public AccountingViewModel Accounting => _AccountingViewModel;

        
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


        private BillingViewModel? _billingViewModel;


        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;
        

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


        private RentViewModel? _rentViewModel;

        #endregion properties & fields


        // constructors
        #region constructors

        public CostsViewModel(AccountingViewModel accountingViewModel)
        {
            _AccountingViewModel = accountingViewModel;

            _AccountingViewModel.AccountingChanged += _AccountingViewModel_AccountingChanged;            

            RentSelected = true;

            Update();
        }

        #endregion constructors


        // methods
        #region methods
        
        private void Update()
        {
            if (_AccountingViewModel.FlatViewModel != null)
            {
                _flatViewModel = _AccountingViewModel.FlatViewModel;

                BillingSelected = false;
            }

            if (_AccountingViewModel.Rents.SelectedValue != null)
            {
                _rentViewModel = _AccountingViewModel.Rents.SelectedValue;
                ActiveViewModel = new RentCostsViewModel(_rentViewModel, _flatViewModel);

                _AccountingViewModel.Rents.UpdateViewModel.RentConfigurationChange += UpdateViewModel_RentConfigurationChange;
            }

            if (_rentViewModel != null && _rentViewModel.BillingViewModel != null)
            {
                _billingViewModel = _rentViewModel.BillingViewModel;
            }
            else
            {
                _billingViewModel = null;
            }

            OnPropertyChanged(nameof(FlatViewModel));
            OnPropertyChanged(nameof(ActiveViewModel));
            OnPropertyChanged(nameof(HasBilling));


            RentSelected = true;

            if (!HasBilling)
            {
                BillingSelected = false;

                RentSelected = true;
            }
        }

        #endregion methods


        // events
        #region events

        private void _AccountingViewModel_AccountingChanged(object? sender, EventArgs e)
        {
            _AccountingViewModel.Rents.SelectedItemChange -= Rents_SelectedItemChange;

            _AccountingViewModel.Rents.SelectedItemChange += Rents_SelectedItemChange;

            Update();
        }

        private void Rents_SelectedItemChange(object? sender, EventArgs e)
        {
            Update();
        }

        private void UpdateViewModel_RentConfigurationChange(object? sender, EventArgs e)
        {
            Update();
        }

        #endregion events


    }
}
// EOF
