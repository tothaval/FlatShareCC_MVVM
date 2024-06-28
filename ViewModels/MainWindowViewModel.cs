using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public FlatManagementViewModel _FlatManagementViewModel { get; set; }


        private string mainWindowTitle;
        public string MainWindowTitle
        {
            get { return mainWindowTitle; }
            set { mainWindowTitle = value; OnPropertyChanged(nameof(MainWindowTitle)); }
        }

        #endregion properties


        // commands
        #region commands

        public ICommand CloseCommand { get; }


        public ICommand LeftPressCommand { get; }

        #endregion properties


        public MainWindowViewModel(ObservableCollection<FlatViewModel> flatViewModels, FlatManagementViewModel flatManagementViewModel)
        {

            _FlatManagementViewModel = flatManagementViewModel;

            CloseCommand = new RelayCommand((s) => Close(), (s) => true);
            LeftPressCommand = new RelayCommand((s) => Drag(s), (s) => true);
        }

        private void Close()
        {
            MessageBoxResult result = MessageBox.Show(
                $"Do you want to close Shared Living Cost Calculator?\n\n",
                "Close SLCC", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }


        private void Drag(object s)
        {
            MainWindow mainWindow = (MainWindow)s;

            mainWindow.DragMove();
        }
    }
}
