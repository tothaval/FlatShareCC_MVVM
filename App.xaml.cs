/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  App 
 * 
 *  manages application startup and exit behaviour
 *  
 *  it loads and saves data
 *  
 *  it instanciates ObservableCollection<FlatViewModel>
 *  for the entire application
 */
using SharedLivingCostCalculator.Enums;
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

        private ObservableCollection<FlatViewModel> _flatCollection;


        private FlatManagementViewModel _managementViewModel;


        private ResourceDictionary _resourceDictionary;

        #endregion properties


        // constructors
        #region constructors

        public App()
        {
            _flatCollection = new ObservableCollection<FlatViewModel>();
            _resourceDictionary = new ResourceDictionary();
            _managementViewModel = new FlatManagementViewModel(_flatCollection);
        }

        #endregion constructors


        // async methods
        #region async methods

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

            PersistanceHandler persistanceHandler = new PersistanceHandler();

            persistanceHandler.SerializeFlatData(_flatCollection);
            persistanceHandler.SerializeResources();

            persistanceHandler.SerializeApplicationState(_managementViewModel);

            //only needed to get a language resource string xml template
            persistanceHandler.SerializeLanguage(); 

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
                DataContext = new MainWindowViewModel(_flatCollection, _managementViewModel)
            };

            mainWindow.Show();

            base.OnStartup(e);
        }

        #endregion methods


    }
}
// EOF