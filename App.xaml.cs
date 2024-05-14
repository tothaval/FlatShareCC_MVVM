using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.Services;
using SharedLivingCostCalculator.ViewModels;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SharedLivingCostCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NavigationStore _navigationStore;
        private MainViewModel _mainViewModel;
        private ObservableCollection<FlatViewModel> _flatCollection;

        public App()
        {
            _navigationStore = new NavigationStore();
            _mainViewModel = new MainViewModel(_navigationStore);
            _flatCollection = new ObservableCollection<FlatViewModel>();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            CreateFlatManagementViewModel().ChangeView();
            
            MainWindow mainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };

            mainWindow.Show();


            base.OnStartup(e);
        }

        private INavigationService CreateFlatManagementViewModel()
        {
            return new NavigationService<FlatManagementViewModel>(_navigationStore, () => new FlatManagementViewModel(_flatCollection, CreateFlatSetupViewModel()));
        }

        private INavigationService CreateFlatSetupViewModel()
        {
            return new NavigationService<FlatSetupViewModel>(_navigationStore, () => new FlatSetupViewModel(_flatCollection, CreateFlatManagementViewModel()));
        }

    }

}
