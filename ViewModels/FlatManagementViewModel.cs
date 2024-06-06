using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Services;
using SharedLivingCostCalculator.Views;
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
    internal class FlatManagementViewModel : BaseViewModel
    {
        private INavigationService _navigationService;

        private string headerText;

        public string HeaderText
        {
            get { return headerText; }
            set { headerText = value; OnPropertyChanged(nameof(HeaderText)); }
        }

        private ObservableCollection<FlatViewModel> _flatCollection;
        public ObservableCollection<FlatViewModel> FlatCollection => _flatCollection;

        private FlatViewModel _selectedValue; // private Billing _selectedBillingPeriod

        public FlatViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                OnPropertyChanged(nameof(SelectedValue));
            }
        }

        public ICommand NewFlatCommand { get; }
        public ICommand SettingsCommand { get; }

        public ICommand AccountingCommand { get; }
        public ICommand EditFlatCommand { get; }
        public ICommand DeleteFlatCommand { get; }



        public FlatManagementViewModel(ObservableCollection<FlatViewModel> flatCollection,
            INavigationService newFlatManagementNavigationService)
        {
            _navigationService = newFlatManagementNavigationService;

            NewFlatCommand = new RelayCommand(NewFlatSetupWindow, CanShowWindow);

            SettingsCommand = new RelayCommand(ShowSettingsWindow, CanShowWindow);

            AccountingCommand = new ShowFlatAccountingCommand(newFlatManagementNavigationService);

            EditFlatCommand = new RelayCommand(ShowFlatSetupWindow, CanShowWindow);

            DeleteFlatCommand = new ExecuteDeleteFlatCommand(flatCollection);

            _flatCollection = flatCollection;

            if (_flatCollection.Count > 0)
            {
                SelectedValue = _flatCollection?.First();
            }
            
            MainWindowTitleText = "Shared Living Cost Calculator - Flat Overview";
            HeaderText = "Flat Overview";
        }

        private bool CanShowWindow(object obj)
        {
            return true;
        }


        private void NewFlatSetupWindow(object obj)
        {
            var mainWindow = Application.Current.MainWindow;

            FlatSetupView flatSetupView = new FlatSetupView();

            flatSetupView.DataContext = new FlatSetupViewModel(_flatCollection, flatSetupView);

            flatSetupView.Owner = mainWindow;
            flatSetupView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            flatSetupView.Show();
        }


        private void ShowFlatSetupWindow(object obj)
        {
            var mainWindow = Application.Current.MainWindow;

            FlatSetupView flatSetupView = new FlatSetupView();
            
            flatSetupView.DataContext = new FlatSetupViewModel(_flatCollection, flatSetupView, SelectedValue);
                      
            flatSetupView.Owner = mainWindow;
            flatSetupView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            flatSetupView.Show();
        }

        private void ShowSettingsWindow(object obj)
        {
            var mainWindow = Application.Current.MainWindow;

            SettingsView settingsView = new SettingsView();

            SettingsViewModel settingsViewModel = new SettingsViewModel();

            settingsView.DataContext = new SettingsViewModel();

            settingsView.Owner = mainWindow;
            settingsView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            settingsView.Show();
        }
    }
}
