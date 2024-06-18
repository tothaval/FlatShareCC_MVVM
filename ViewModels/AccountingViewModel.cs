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

        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;


        private RentManagementViewModel _rents;
        public RentManagementViewModel Rents => _rents;


        public ICommand LeaveCommand { get; }


        public string Address => _flatViewModel.Address;


        public string Details => _flatViewModel.Details;


        public double Area  => _flatViewModel.Area;


        public int RoomCount => _flatViewModel.RoomCount;


        public double FontSize => (double)App.Current.FindResource("FS") * 2;


        public AccountingViewModel(FlatViewModel flatViewModel, INavigationService navigationService)
        {
            _flatViewModel = flatViewModel;
            LeaveCommand = new NavigateCommand(navigationService);

            _rents = new RentManagementViewModel(FlatViewModel);
        }


    }
}
// EOF