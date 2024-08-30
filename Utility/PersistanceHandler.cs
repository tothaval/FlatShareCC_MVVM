/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PersistanceHandler 
 * 
 *  helper class for serializing data
 */
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;


namespace SharedLivingCostCalculator.Utility
{
    internal class PersistanceHandler
    {

        // constructors
        #region constructors

        public PersistanceHandler()
        {
            
        }

        #endregion constructors


        // methods
        #region methods

        internal void SaveData(FlatManagementViewModel flatManagementViewModel)
        {
            SerializeFlatData(flatManagementViewModel.FlatCollection);
            SerializeResources();

            SerializeApplicationState(flatManagementViewModel);

            //only needed to get a language resource string xml template
            SerializeLanguage();

        }


        internal void SerializeApplicationState(BaseViewModel currentViewModel)
        {

            var xmlSerializer = new XmlSerializer(typeof(ApplicationData));

            ApplicationData applicationData = new ApplicationData(currentViewModel);

            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\appdata\\";

            using (var writer = new StreamWriter($"{folder}\\appdata.xml"))
            {
                xmlSerializer.Serialize(writer, applicationData);
            }
        }


        public void SerializeFlatData(ObservableCollection<FlatViewModel> flats)
        {
            for (int i = 0; i < flats.Count; i++)
            {
                var xmlSerializer = new XmlSerializer(typeof(PersistanceDataSet));

                using (var writer = new StreamWriter($"{i}.xml"))
                {
                    PersistanceDataSet persistanceDataSet = new PersistanceDataSet(flats[i]);
                    xmlSerializer.Serialize(writer, persistanceDataSet);
                }
            }
        }


        public void SerializeLanguage(string language = "English")
        {
            var xmlSerializer = new XmlSerializer(typeof(LanguageResourceStrings));
            LanguageResourceStrings LRS = new LanguageResourceStrings(language);

            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\language\\";

            using (var writer = new StreamWriter($"{folder}\\{language}.xml"))
            {
                xmlSerializer.Serialize(writer, LRS);
            }
        }


        public void SerializeResources()
        {
            var xmlSerializer = new XmlSerializer(typeof(Resources));
            Resources resources = new Resources().GetResources();


            using (var writer = new StreamWriter("resources.xml"))
            {
                xmlSerializer.Serialize(writer, resources);
            }
        }

        #endregion methods


    }
}
// EOF