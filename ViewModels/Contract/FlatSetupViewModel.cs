/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  FlatSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for FlatSetupView
 *  
 *  displays a seperate window for creating
 *  or editing of _FlatViewModel instances
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Contract
{
    public class FlatSetupViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;

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

            _FlatViewModel = _FlatManagementViewModel.SelectedItem;

            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;

            OnPropertyChanged(nameof(FlatViewModel));
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

                _FlatViewModel.PropertyChanged += _flatsetup_PropertyChanged;
            }

            OnPropertyChanged(nameof(FlatViewModel));
        }

        private void _flatsetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FlatViewModel));
        }

        #endregion events


    }
}
// EOF