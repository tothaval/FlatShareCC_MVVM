using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.Services;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

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
        private ResourceDictionary _resourceDictionary;

        public App()
        {
            _navigationStore = new NavigationStore();
            _mainViewModel = new MainViewModel(_navigationStore);
            _flatCollection = new ObservableCollection<FlatViewModel>();
            _resourceDictionary = new ResourceDictionary();

            Models.Flat flat = new Models.Flat()
            {
                ID = 0,
                Address = "Testadresse 4, 01234 Testort",
                Details = "Mittelhaus, 5. Etage",
                Area = 113.57,
                RoomCount = 5,
                Rooms = new ObservableCollection<Models.Room>
                {
                    new Models.Room()
                    {
                        ID = 0,
                        RoomName = "Kinderzimmer 1",
                        RoomArea = 13
                    },
                    new Models.Room()
                    {
                        ID = 0,
                        RoomName = "Kinderzimmer 2",
                        RoomArea = 17
                    },
                    new Models.Room()
                    {
                        ID = 0,
                        RoomName = "Arbeitszimmer",
                        RoomArea = 12
                    },
                    new Models.Room()
                    {
                        ID = 0,
                        RoomName = "Schlafzimmer",
                        RoomArea = 19
                    },
                    new Models.Room()
                    {
                        ID = 0,
                        RoomName = "Wohnzimmer",
                        RoomArea = 27
                    },
                }
            };

            FlatViewModel flatViewModel = new FlatViewModel(flat);

            flat.RentUpdates.Add(new Models.Rent(flatViewModel)
            {
                StartDate = DateTime.Now,
                ColdRent = 940.87,
                ExtraCostsShared = 197.25,
                ExtraCostsHeating = 328.55
            });
       

            _flatCollection.Add(new FlatViewModel(flat));


            _flatCollection.Add(new FlatViewModel(
                new Models.Flat(
                    1, "Addresse 1", 128, 5, new ObservableCollection<Models.Room>() {
            new Models.Room(0, "zimmer 0", 22.4),
            new Models.Room(1, "zimmer 1", 16.8),
            new Models.Room(2, "zimmer 2", 12.4),
            new Models.Room(3, "zimmer 3", 17),
            new Models.Room(4, "zimmer 4", 19) },
        "vorderhaus")));
            _flatCollection.Add(new FlatViewModel(
    new Models.Flat(
        2, "Addresse 2, 00770 stadt", 109, 4, new ObservableCollection<Models.Room>() {
            new Models.Room(0, "ZIMMER 00", 25),
            new Models.Room(1, "ZIMMER 01", 14),
            new Models.Room(2, "ZIMMER 02", 13),
            new Models.Room(3, "ZIMMER 03", 17)},
        "mittelhaus")));
            _flatCollection.Add(new FlatViewModel(
    new Models.Flat(
        3, "fancy address 3", 78, 3, new ObservableCollection<Models.Room>() {
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

            AddResources();

        }

        private INavigationService CreateFlatManagementViewModel()
        {
            return new NavigationService<FlatManagementViewModel>(_navigationStore, () => new FlatManagementViewModel(_flatCollection, CreateFlatManagementViewModel()));
        }

        private void AddResources()
        {
            _resourceDictionary.Add("R_Background", new SolidColorBrush(Colors.White));
            _resourceDictionary.Add("R_Foreground", new SolidColorBrush(Colors.Black));
            _resourceDictionary.Add("R_Header", new SolidColorBrush(Colors.Black));
            _resourceDictionary.Add("R_FontFamily", new FontFamily("Segoe"));
            _resourceDictionary.Add("R_FontSize", (double)11);
            
            // once the main functionality is finished, integrate a way to change cultureInfo/Currency 
            // binding it as a dynamic resource does not seem to work so far.
            //_resourceDictionary.Add("Culture", CultureInfo.CurrentCulture = new CultureInfo("de-DE"));

            Application.Current.Resources["R_Background"] = new SolidColorBrush(Colors.White);
            Application.Current.Resources["R_Foreground"] = new SolidColorBrush(Colors.Black);
            Application.Current.Resources["R_Header"] = new SolidColorBrush(Colors.Green);
            Application.Current.Resources["R_FontFamiliy"] = new FontFamily("Segoe");
            Application.Current.Resources["R_FontSize"] = (double)11;
            
            //Application.Current.Resources["Culture"] = new CultureInfo("de-DE");

        }
    }

}
