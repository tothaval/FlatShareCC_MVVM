/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PersistanceHandler 
 * 
 *  helper class for serializing data
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.ViewModels;
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


        public void SerializeLanguage(SupportedLanguages language)
        {
            var xmlSerializer = new XmlSerializer(typeof(LanguageResourceStrings));
            LanguageResourceStrings LRS = new LanguageResourceStrings(language);

            string lang = language.ToString();
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\language\\";

            using (var writer = new StreamWriter($"{folder}\\{lang}.xml"))
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