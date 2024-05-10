using System.Configuration;
using System.Data;
using System.Windows;
using WGMietkosten.Models;
using WGMietkosten.Navigation;
using WGMietkosten.ViewModels;

namespace WGMietkosten
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private FlatManagement _flatManagement;
        private NavigationStore _navigationStore;

        public App()
        {
            _flatManagement = new FlatManagement();
            _navigationStore = new NavigationStore(ref _flatManagement);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new FlatManagementViewModel(_navigationStore);

            MainWindow mainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            mainWindow.Show();




            base.OnStartup(e);
        }
    }

}
