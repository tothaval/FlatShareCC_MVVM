using PropertyTools.Wpf.Shell32;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.Services;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

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

        private readonly INavigationService _NavigateToFlatManagementViewModel;

        public App()
        {
            _navigationStore = new NavigationStore();
            _mainViewModel = new MainViewModel(_navigationStore);
            _flatCollection = new ObservableCollection<FlatViewModel>();
            _resourceDictionary = new ResourceDictionary();

            _NavigateToFlatManagementViewModel = new NavigationService<FlatManagementViewModel>(_navigationStore, () => new FlatManagementViewModel(_flatCollection, _NavigateToFlatManagementViewModel));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            LoadData();

            _NavigateToFlatManagementViewModel.ChangeView();

            MainWindow mainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };

            mainWindow.Show();
                        
            base.OnStartup(e);
        }

        private void AddResources()
        {
            new Resources();
        }


        private async void LoadData()
        {
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string filter = "*.xml";

            List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();

            if (!Directory.EnumerateFiles(folder).Any(f => f.Contains("resources.xml")))
            {
                AddResources();
            }

            foreach (string file in files)
            {
                if (!file.EndsWith("resources.xml"))
                {
                    var xmlSerializer = new XmlSerializer(typeof(PersistanceDataSet));

                    using (var writer = new StreamReader(file))
                    {
                        try
                        {
                            var member = (PersistanceDataSet)xmlSerializer.Deserialize(writer);

                            FlatViewModel flatViewModel = await member.GetFlatData();

                            _flatCollection.Add(flatViewModel);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (file.EndsWith("resources.xml"))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Resources));

                    using (var writer = new StreamReader(file))
                    {
                        try
                        {
                            var member = (Resources)xmlSerializer.Deserialize(writer);

                            member.SetResources();
                                                       
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private async void CleanFolder()
        {
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string filter = "*.xml";

            List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();

            if (files.Count > _flatCollection.Count)
            {
                foreach (string file in files)
                {
                    if (!file.EndsWith("resources.xml"))
                    {
                        File.Delete(file);
                    }
                }
            }

            await Task.Delay(15);
        }


        protected override void OnExit(ExitEventArgs e)
        {
            CleanFolder();

            PersistanceHandler persistanceHandler = new PersistanceHandler();

            persistanceHandler.SerializeFlatData(_flatCollection);
            persistanceHandler.SerializeResources();

            base.OnExit(e);
        }

    }

}
