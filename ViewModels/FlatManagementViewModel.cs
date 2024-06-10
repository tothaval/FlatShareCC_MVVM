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

        public string FlatManagementInstructionText { get; set; }

        private ObservableCollection<FlatViewModel> _flatCollection;
        public ObservableCollection<FlatViewModel> FlatCollection
        {
            get { return _flatCollection; }
            set
            {
                if (_flatCollection == value) return;
                _flatCollection = value;

                OnPropertyChanged(nameof(FlatCollection));
            }
        }



        private FlatViewModel _SelectedItem;
        public FlatViewModel SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                if (_SelectedItem == value) return;
                _SelectedItem = value;

                if (_flatCollection.Count > 0 && _SelectedItem != null)
                {
                    _SelectedItem.SetMostRecentCosts();
                }

                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public bool HasFlat => _flatCollection.Count > 0;

        private bool _FlatCollectionFilled;
        public bool FlatCollectionFilled
        {
            get { return _FlatCollectionFilled; }
            set
            {
                _FlatCollectionFilled = value;
                OnPropertyChanged(nameof(FlatCollectionFilled));
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
                SelectedItem = _flatCollection?.First();
                FlatCollectionFilled = true;

                _flatCollection.CollectionChanged += _flatCollection_CollectionChanged;
            }
            
            MainWindowTitleText = "Shared Living Cost Calculator - Flat Overview";
            HeaderText = "Flat Overview";

            FlatManagementInstructionText = InstructionText();
        }

        private void _flatCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FlatCollectionFilled = FlatCollection.Count > 0;

            if (_flatCollection.Count > 0)
            {
                SelectedItem = _flatCollection?.First();
                FlatCollectionFilled = true;
            }
            OnPropertyChanged(nameof(HasFlat));
        }

        private bool CanShowWindow(object obj)
        {
            return true;
        }

        private string InstructionText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
                "Flat Management\n" +
                "\n" +
                "-> click \"New Flat\" to show flat setup window\n" +
                "\t setup flat, !!! address, area(s) and room counts can not be edited\n" +
                "\t -> click \"Proceed\" to create new flat\n" +
                "\t -> click \"Leave\" to return to flat management\n" +
                "-> select flat to view its most recent data\n" +
                "-> click \"Delete\" to delete selected flat\n" +
                "-> click \"Accounting\" to enter accounting for selected flat\n" +
                "-> click \"Settings\" to show settings window\n"
                );

            return stringBuilder.ToString();
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
            
            flatSetupView.DataContext = new FlatSetupViewModel(_flatCollection, flatSetupView, SelectedItem);
                      
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
