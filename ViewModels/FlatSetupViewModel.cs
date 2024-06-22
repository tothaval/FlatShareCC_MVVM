﻿/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  FlatSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for FlatSetupView
 *  
 *  displays a seperate window for creating
 *  or editing of FlatViewModel instances
 */
using System.Collections.ObjectModel;
using System.Windows.Input;
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using SharedLivingCostCalculator.Views.Windows;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class FlatSetupViewModel : BaseViewModel
    {

        private FlatViewModel _flatsetup;
        public FlatViewModel FlatSetup => _flatsetup;


        private FlatManagementViewModel _FlatManagementViewModel;

        
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
// EOF