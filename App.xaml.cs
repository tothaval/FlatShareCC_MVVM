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

            _flatCollection.Add(new FlatViewModel(
                new Models.Flat(
                    0, "Addresse 1", 128, 5, new ObservableCollection<Models.Room>() {
            new Models.Room(0, "zimmer 0", 22.4),
            new Models.Room(1, "zimmer 1", 16.8),
            new Models.Room(2, "zimmer 2", 12.4),
            new Models.Room(3, "zimmer 3", 17),
            new Models.Room(4, "zimmer 4", 19) },
        "vorderhaus")));
            _flatCollection.Add(new FlatViewModel(
    new Models.Flat(
        1, "Addresse 2, 00770 stadt", 109, 4, new ObservableCollection<Models.Room>() {
            new Models.Room(0, "ZIMMER 00", 25),
            new Models.Room(1, "ZIMMER 01", 14),
            new Models.Room(2, "ZIMMER 02", 13),
            new Models.Room(3, "ZIMMER 03", 17)} ,
        "mittelhaus")));
            _flatCollection.Add(new FlatViewModel(
    new Models.Flat(
        2, "fancy address 3", 78, 3, new ObservableCollection<Models.Room>() {
            new Models.Room(0, "Room 0", 19),
            new Models.Room(1, "Room 1", 15),
            new Models.Room(2, "Room 2", 12.4) },
        "5.og rechts")));
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
            return new NavigationService<FlatManagementViewModel>(_navigationStore, () => new FlatManagementViewModel(_flatCollection, CreateFlatSetupViewModel(), CreateFlatManagementViewModel()));
        }

        private INavigationService CreateFlatSetupViewModel()
        {
            return new NavigationService<FlatSetupViewModel>(_navigationStore, () => new FlatSetupViewModel(_flatCollection, CreateFlatManagementViewModel()));
        }

    }

}
