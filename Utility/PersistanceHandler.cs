using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
