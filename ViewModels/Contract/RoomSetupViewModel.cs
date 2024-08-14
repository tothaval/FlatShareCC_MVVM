/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for RoomSetupView
 *  
 *  allows editing of RoomViewModel
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Contract
{
    public class RoomSetupViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

        private FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;

        #endregion properties


        // constructors
        #region constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flatManagementViewModel"></param>
        public RoomSetupViewModel(FlatManagementViewModel flatManagementViewModel)
        {
            _FlatManagementViewModel = flatManagementViewModel;

            _FlatViewModel = _FlatManagementViewModel.SelectedItem;

            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;
        }

        #endregion constructors


        // methods
        #region methods

        private void _flatsetup_RoomCreation()
        {
            FlatViewModel.ConnectRooms();

            OnPropertyChanged(nameof(FlatViewModel));
        }

        #endregion methods


        // events
        #region events

        private void _FlatManagementViewModel_FlatViewModelChange(object? sender, EventArgs e)
        {
            _FlatViewModel = _FlatManagementViewModel.SelectedItem;

            if (_FlatViewModel != null)
            {
                _FlatViewModel.RoomCreation += _flatsetup_RoomCreation;
            }

            OnPropertyChanged(nameof(FlatViewModel));
        }

        #endregion events


    }
}
// EOF