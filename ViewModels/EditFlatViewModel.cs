using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    class EditFlatViewModel : BaseViewModel
    {
        private ObservableCollection<FlatViewModel> _flatCollection;
        private FlatViewModel _flatsetup;
        public FlatViewModel FlatSetup => _flatsetup;

        public bool FlatSetupCommandVisibility => false;

        public ICommand FlatSetupCommand { get; }
        public ICommand LeaveViewCommand { get; }


        public EditFlatViewModel(FlatViewModel flatViewModel, INavigationService flatManagementNavigationService)
        {
            _flatsetup = flatViewModel;
            
            MainWindowTitleText = "Shared Living Cost Calculator - Edit Flat";

            LeaveViewCommand = new NavigateCommand(flatManagementNavigationService);

            OnPropertyChanged(nameof(FlatSetupCommandVisibility));
        }
    }
}
