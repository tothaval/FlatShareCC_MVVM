/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  FlatSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for FlatSetupView
 *  
 *  displays a seperate window for creating
 *  or editing of FlatViewModel instances
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels
{
    public class FlatSetupViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _flatsetup;
        public FlatViewModel FlatSetup => _flatsetup;

        #endregion properties & fields


        // constructors
        #region constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="flatManagementViewModel"></param>
        public FlatSetupViewModel(FlatManagementViewModel flatManagementViewModel)
        {
            _FlatManagementViewModel = flatManagementViewModel;

            _flatsetup = _FlatManagementViewModel.SelectedItem;

            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;
        }

        #endregion constructors


        // methods
        #region methods

        private void _flatsetup_RoomCreation()
        {
            FlatSetup.ConnectRooms();

            OnPropertyChanged(nameof(FlatSetup));
        }

        #endregion methods


        // events
        #region events

        private void _FlatManagementViewModel_FlatViewModelChange(object? sender, EventArgs e)
        {
            _flatsetup = _FlatManagementViewModel.SelectedItem;

            if (_flatsetup != null)
            {
                _flatsetup.RoomCreation += _flatsetup_RoomCreation;
            }

            OnPropertyChanged(nameof(FlatSetup));
        }

        #endregion events


    }
}
// EOF