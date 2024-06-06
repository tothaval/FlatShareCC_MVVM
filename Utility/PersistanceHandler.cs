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
        [XmlIgnore]
        private ObservableCollection<PersistanceDataSet> _persistanceDataSet;

        public void Serialize()
        {
            for (int i = 0; i < _persistanceDataSet.Count; i++)
            {
                var xmlSerializer = new XmlSerializer(typeof(PersistanceDataSet));

                using (var writer = new StreamWriter($"{i}.xml"))
                {
                    xmlSerializer.Serialize(writer, _persistanceDataSet[i]);
                }
            }
        }


        public PersistanceHandler(ObservableCollection<FlatViewModel> flats)
        {
            _persistanceDataSet = new ObservableCollection<PersistanceDataSet>();

            foreach (FlatViewModel item in flats)
            {
                _persistanceDataSet.Add(new PersistanceDataSet(item)); ;

            }
        }
    }
}
