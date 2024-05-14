using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Services;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class FlatSetupViewModel : BaseViewModel
    {
        private ObservableCollection<FlatViewModel> _flatCollection;
        private FlatViewModel _flatsetup;
        public FlatViewModel FlatSetup => _flatsetup;

        public ICommand FlatSetupCommand {  get; }
        public ICommand LeaveViewCommand {  get; }

        public FlatSetupViewModel(ObservableCollection<FlatViewModel> flatCollection, INavigationService flatManagementNavigationService)
        {
            _flatCollection = flatCollection;

            _flatsetup = new FlatViewModel(new Flat());

            MainWindowTitleText = "Shared Living Cost Calculator - Flat Setup";

            FlatSetupCommand = new FlatSetupCommand(_flatCollection, this, flatManagementNavigationService);
            LeaveViewCommand = new NavigateCommand(flatManagementNavigationService);

            _flatsetup.RoomCreation += _flatsetup_RoomCreation;
            
        }

        private void _flatsetup_RoomCreation()
        {
            OnPropertyChanged(nameof(FlatSetup));
        }
    }
}
