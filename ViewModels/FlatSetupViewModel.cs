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
using SharedLivingCostCalculator.Views;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class FlatSetupViewModel : BaseViewModel
    {
        private ObservableCollection<FlatViewModel> _flatCollection;
        private FlatViewModel _flatsetup;
        public FlatViewModel FlatSetup => _flatsetup;
        private FlatSetupView _flatSetupView;

        public bool FlatSetupCommandVisibility => true;


        private string _flatSetupTitleText;

        public string FlatSetupTitleText
        {
            get { return _flatSetupTitleText; }
            set
            {
                _flatSetupTitleText = value;
                OnPropertyChanged(nameof(FlatSetupTitleText));
            }
        }



        public ICommand FlatSetupCommand { get; }
        public ICommand LeaveViewCommand { get; }


        public FlatSetupViewModel(ObservableCollection<FlatViewModel> flatCollection, FlatSetupView flatSetupView)
        {
            _flatCollection = flatCollection;
            _flatSetupView = flatSetupView;

            _flatsetup = new FlatViewModel(new Flat());

            FlatSetupTitleText = "Shared Living Cost Calculator - Flat Setup";

            FlatSetupCommand = new FlatSetupCommand(_flatCollection, this, flatSetupView);
            LeaveViewCommand = new RelayCommand(Close, (s) => true);

            _flatsetup.RoomCreation += _flatsetup_RoomCreation;

            OnPropertyChanged(nameof(FlatSetupCommandVisibility));
        }


        public FlatSetupViewModel(ObservableCollection<FlatViewModel> flatCollection, FlatSetupView flatSetupView,
                FlatViewModel flatViewModel)
        {
            _flatCollection = flatCollection;
            _flatSetupView = flatSetupView;

            _flatsetup = flatViewModel;

            FlatSetupTitleText = "Shared Living Cost Calculator - Flat Setup";

            FlatSetupCommand = new FlatSetupCommand(_flatCollection, this, flatSetupView);
            LeaveViewCommand = new RelayCommand(Close, (s) => true);

            _flatsetup.RoomCreation += _flatsetup_RoomCreation;

            OnPropertyChanged(nameof(FlatSetupCommandVisibility));
        }


        private void Close(object obj)
        {
            _flatSetupView.Close();
        }

        private void _flatsetup_RoomCreation()
        {
            OnPropertyChanged(nameof(FlatSetup));
        }
    }
}
