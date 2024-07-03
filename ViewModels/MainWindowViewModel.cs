/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainWindowViewModel  : BaseViewModel
 * 
 *  viewmodel for MainWindow
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
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


        public ICommand LeftDoubleClickCommand { get; }


        public ICommand LeftPressCommand { get; }


        public ICommand MinimizeCommand { get; }

        #endregion commands


        public MainWindowViewModel(ObservableCollection<FlatViewModel> flatViewModels, FlatManagementViewModel flatManagementViewModel)
        {

            _FlatManagementViewModel = flatManagementViewModel;

            CloseCommand = new RelayCommand((s) => Close(s), (s) => true);
            LeftDoubleClickCommand = new RelayCommand((s)=> Maximize(s), (s) => true);
            LeftPressCommand = new RelayCommand((s) => Drag(s), (s) => true);
            MinimizeCommand = new RelayCommand((s) => Minimize(s), (s) => true);
        }


        private void Close(object s)
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


        private async void Maximize(object s)
        {
            MainWindow mainWindow = (MainWindow)s;

            mainWindow.SizeToContent = SizeToContent.Manual;

            if (mainWindow.WindowState == WindowState.Normal)
            {
                mainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                mainWindow.SizeToContent = SizeToContent.WidthAndHeight;

                await Task.Delay(100);

                mainWindow.WindowState = WindowState.Normal;

                mainWindow.BorderThickness = new Thickness(0);

                mainWindow.BorderThickness = new Thickness(4);
            }

        }


        private void Minimize(object s)
        {
            // i don't get it, why is s null and not null on Maximize?
            Window window = (Window)s;

            window.WindowState = WindowState.Minimized;
        }


    }
}
// EOF