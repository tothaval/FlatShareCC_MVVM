using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels
{
    class RoomSetupViewModel : BaseViewModel
    {

        private FlatViewModel _flatsetup;
        public FlatViewModel FlatSetup => _flatsetup;


        private FlatManagementViewModel _FlatManagementViewModel;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="flatManagementViewModel"></param>
        public RoomSetupViewModel(FlatManagementViewModel flatManagementViewModel)
        {
            _FlatManagementViewModel = flatManagementViewModel;

            _flatsetup = _FlatManagementViewModel.SelectedItem;

            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;
        }


        private void _FlatManagementViewModel_FlatViewModelChange(object? sender, EventArgs e)
        {
            _flatsetup = _FlatManagementViewModel.SelectedItem;

            if (_flatsetup != null)
            {
                _flatsetup.RoomCreation += _flatsetup_RoomCreation;
            }

            OnPropertyChanged(nameof(FlatSetup));
        }



        private void _flatsetup_RoomCreation()
        {
            FlatSetup.ConnectRooms();

            OnPropertyChanged(nameof(FlatSetup));
        }

    }
}
