/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  FlatSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for FlatSetupView
 *  
 *  displays a seperate window for creating
 *  or editing of _FlatViewModel instances
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Contract
{
    public class FlatSetupViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        private bool _UseRoomsChecked;
        public bool UseRoomsChecked
        {
            get { return _UseRoomsChecked; }
            set
            {
                _UseRoomsChecked = value;
                OnPropertyChanged(nameof(UseRoomsChecked));
            }
        }


        private bool _UseWorkspacesChecked;
        public bool UseWorkspacesChecked
        {
            get { return _UseWorkspacesChecked; }
            set
            {
                _UseWorkspacesChecked = value;
                OnPropertyChanged(nameof(UseWorkspacesChecked));
            }
        }

        #endregion properties & fields


        // Commands
        #region Commands

        public ICommand UseRoomsCommand { get; }


        public ICommand UseWorkspacesCommand { get; }

        #endregion


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

            UseRoomsCommand = new RelayCommand((s) => UseRooms(), (s) => true);

            UseWorkspacesCommand = new RelayCommand((s) => UseWorkplaces(), (s) => true);

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


        private void UseRooms()
        {
            UseWorkspacesChecked = false;
            UseRoomsChecked = true;

            if (FlatViewModel != null)
            {
                FlatViewModel.UseRooms();
            }
        }


        private void UseWorkplaces()
        {
            UseWorkspacesChecked = true;
            UseRoomsChecked = false;

            if (FlatViewModel != null)
            {
                FlatViewModel.UseWorkplaces();
            }
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

                UseRoomsChecked = _FlatViewModel.Flat.UseRooms;
                UseWorkspacesChecked = _FlatViewModel.Flat.UseWorkspaces;
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