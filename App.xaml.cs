/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  App 
 * 
 *  manages application startup and exit behaviour
 *  
 *  it loads and saves data
 *  
 *  it instanciates ObservableCollection<_FlatViewModel>
 *  for the entire application
 */
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Serialization;


namespace SharedLivingCostCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // properties
        #region properties

        private ObservableCollection<FlatViewModel> _FlatCollection;


        private FlatManagementViewModel _FlatManagementViewModel;


        private ResourceDictionary _ResourceDictionary;

        #endregion properties


        // constructors
        #region constructors

        public App()
        {
            _FlatCollection = new ObservableCollection<FlatViewModel>();
            _ResourceDictionary = new ResourceDictionary();
            _FlatManagementViewModel = new FlatManagementViewModel(_FlatCollection);
        }

        #endregion constructors


        // async methods
        #region async methods

        private async void CleanFolder()
        {
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string filter = "*.xml";

            List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();

            if (files.Count > _FlatCollection.Count)
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

                            _FlatCollection.Add(flatViewModel);
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

        #endregion async methods


        // methods
        #region methods

        private void AddResources()
        {
            new Resources();
        }


        protected override void OnExit(ExitEventArgs e)
        {
            CleanFolder();

            new PersistanceHandler().SaveData(_FlatManagementViewModel);

            base.OnExit(e);
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            string appdata_folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\appdata\\";
            string language_folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\language\\";

            if (!Directory.Exists(appdata_folder))
            {
                Directory.CreateDirectory(appdata_folder);
            }


            if (!Directory.Exists(language_folder))
            {
                Directory.CreateDirectory(language_folder);

                new PersistanceHandler().SerializeLanguage();

            }

            LoadData();


            MainWindow mainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(_FlatCollection, _FlatManagementViewModel)
            };

            mainWindow.Show();

            base.OnStartup(e);
        }

        #endregion methods


    }
}
// EOF