/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  AccountingViewModel : BaseViewModel
 * 
 *  viewmodel for AccountingView
 *  
 *  displays the selected _FlatViewModel data
 *  
 *  holds a TabControl to access
 *  RentManagementviewModel,
 *  BillingManagementViewModel,
 *  PaymentManagementViewModel
 */
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class AccountingViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

        public string Address => _FlatViewModel.Address;


        public double Area => _FlatViewModel.Area;


        public string Details => _FlatViewModel.Details;


        private readonly FlatManagementViewModel _FlatManagementViewModel;
        public FlatManagementViewModel FlatManagement => _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


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


        public int RoomCount => _FlatViewModel.RoomCount;

        #endregion


        // Event Properties & Fields
        #region Event Properties & Fields

        public event EventHandler AccountingChanged;

        #endregion


        // Constructors
        #region Constructors

        public AccountingViewModel(FlatManagementViewModel flatManagementViewModel)
        {
            _FlatManagementViewModel = flatManagementViewModel;

            if (_FlatManagementViewModel.SelectedItem == null)
            {
                _FlatViewModel = new FlatViewModel(new Flat());
            }
            else
            {
                _FlatViewModel = _FlatManagementViewModel.SelectedItem;
            }

            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;

            Rents = new RentManagementViewModel(this);
        }

        #endregion


        // Events
        #region Events

        private void _FlatManagementViewModel_FlatViewModelChange(object? sender, EventArgs e)
        {
            _FlatViewModel = _FlatManagementViewModel.SelectedItem;

            Rents = new RentManagementViewModel(this);

            OnPropertyChanged(nameof(Address));
            OnPropertyChanged(nameof(Details));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(RoomCount));
            OnPropertyChanged(nameof(FlatViewModel));
        }

        #endregion


    }
}
// EOF