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
using SharedLivingCostCalculator.Interfaces;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Windows.Input;


namespace SharedLivingCostCalculator.ViewModels
{
    class AccountingViewModel : BaseViewModel
    {

        private readonly FlatManagementViewModel _FlatManagementViewModel;
        public FlatManagementViewModel FlatManagement => _FlatManagementViewModel;


        public event EventHandler AccountingChanged;


        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;


        private RentManagementViewModel _Rents;
        public RentManagementViewModel Rents
        {
            get { return _Rents; }
            set
            {
                _Rents = value;
                OnPropertyChanged(nameof(Rents));

                AccountingChanged?.Invoke(this, EventArgs.Empty);
            }
        }



        public string Address => _flatViewModel.Address;


        public string Details => _flatViewModel.Details;


        public double Area  => _flatViewModel.Area;


        public int RoomCount => _flatViewModel.RoomCount;


        public string FlatNotes => _flatViewModel.FlatNotes;


        public AccountingViewModel(FlatManagementViewModel flatManagementViewModel)
        {
            _FlatManagementViewModel = flatManagementViewModel;

            if (_FlatManagementViewModel.SelectedItem == null)
            {
                _flatViewModel = new FlatViewModel(new Flat());                
            }
            else
            {
                _flatViewModel = _FlatManagementViewModel.SelectedItem;
            }

            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;

            Rents = new RentManagementViewModel(this);
        }


        private void _FlatManagementViewModel_FlatViewModelChange(object? sender, EventArgs e)
        {
            _flatViewModel = _FlatManagementViewModel.SelectedItem;

            Rents = new RentManagementViewModel(this);

            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(Details));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(RoomCount));

        }


    }
}
// EOF