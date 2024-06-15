/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PersistanceHandler 
 * 
 *  helper class for serializing data
 */
using SharedLivingCostCalculator.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;


namespace SharedLivingCostCalculator.Utility
{
    internal class PersistanceHandler
    {

        public void SerializeFlatData(ObservableCollection<FlatViewModel> flats)
        {
            for (int i = 0; i < flats.Count; i++)
            {
                var xmlSerializer = new XmlSerializer(typeof(PersistanceDataSet));

                using (var writer = new StreamWriter($"{i}.xml"))
                {
                    xmlSerializer.Serialize(writer, new PersistanceDataSet(flats[i]));
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


        public PersistanceHandler()
        {
            
        }


    }
}
// EOF