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
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels
{
    public class AccountingViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public string Address => _flatViewModel.Address;


        public double Area => _flatViewModel.Area;


        public string Details => _flatViewModel.Details;


        private readonly FlatManagementViewModel _FlatManagementViewModel;
        public FlatManagementViewModel FlatManagement => _FlatManagementViewModel;


        public string FlatNotes => _flatViewModel.FlatNotes;


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


        public int RoomCount => _flatViewModel.RoomCount;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler AccountingChanged;

        #endregion event properties & fields


        // constructors
        #region constructors

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

        #endregion constructors


        // events
        #region events

        private void _FlatManagementViewModel_FlatViewModelChange(object? sender, EventArgs e)
        {
            _flatViewModel = _FlatManagementViewModel.SelectedItem;

            Rents = new RentManagementViewModel(this);

            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(Details));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(RoomCount));

        }

        #endregion events


    }
}
// EOF