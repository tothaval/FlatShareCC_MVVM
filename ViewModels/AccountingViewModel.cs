/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  AccountingViewModel : BaseViewModel
 * 
 *  viewmodel for AccountingView
 *  
 *  displays the selected FlatViewModel data
 *  
 *  holds a TabControl to access
 *  RentManagementviewModel,
 *  BillingManagementViewModel,
 *  PaymentManagementViewModel
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Services;
using System.Windows.Input;


namespace SharedLivingCostCalculator.ViewModels
{
    class AccountingViewModel : BaseViewModel
    {

        public bool SelectionHasBilling
        {
            get
            {
                if (_rents.UpdateViewModel != null)
                {
                    return _rents.UpdateViewModel.HasBilling;
                }

                return false;
            }
        }


        public bool SelectionHasCredit
        {
            get
            {
                if (_rents.UpdateViewModel != null)
                {
                    return _rents.UpdateViewModel.HasCredit;
                }

                return false;
            }
        }


        public bool SelectionHasOtherCosts
        {
            get
            {
                if (_rents.UpdateViewModel != null)
                {
                    return _rents.UpdateViewModel.HasOtherCosts;
                }

                return false;
            }
        }


        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;


        private RentManagementViewModel _rents;
        public RentManagementViewModel Rents => _rents;


        private BillingManagementViewModel _billings;
        public BillingManagementViewModel Billings => _billings;


        private PaymentManagementViewModel _payments;
        public PaymentManagementViewModel Payments => _payments;


        public ICommand LeaveCommand { get; }


        public string Address => _flatViewModel.Address;


        public string Details => _flatViewModel.Details;


        public double Area  => _flatViewModel.Area;


        public int RoomCount => _flatViewModel.RoomCount;


        public double FontSize => (double)App.Current.FindResource("FS") * 2;


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


        public AccountingViewModel(FlatViewModel flatViewModel, INavigationService navigationService)
        {
            _flatViewModel = flatViewModel;
            LeaveCommand = new NavigateCommand(navigationService);

            _rents = new RentManagementViewModel(FlatViewModel);

            _billings = new BillingManagementViewModel(FlatViewModel, this);
            _payments = new PaymentManagementViewModel(FlatViewModel);

            registerEvent();

            _rents.PropertyChanged += _rents_PropertyChanged;
        }


        private void _rents_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            registerEvent();
            updateOnPropertyChanged();
        }


        private void UpdateViewModel_RentConfigurationChange(object? sender, EventArgs e)
        {
            updateOnPropertyChanged();
        }


        private void registerEvent()
        {
            if (_rents.UpdateViewModel != null)
            {
                _rents.UpdateViewModel.RentConfigurationChange += UpdateViewModel_RentConfigurationChange;
            }
        }


        private void updateOnPropertyChanged()
        {
            OnPropertyChanged(nameof(SelectionHasBilling));
            OnPropertyChanged(nameof(SelectionHasCredit));
            OnPropertyChanged(nameof(SelectionHasOtherCosts));
        }
    }
}
// EOF