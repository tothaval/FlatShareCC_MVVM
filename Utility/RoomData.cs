using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Utility
{
    [Serializable]
    public class RoomData
    {
        public int ID { get; set; } = -1;
        public string RoomName { get; set; } = string.Empty;

        public double RoomArea { get; set; } = 0.0;

        public ObservableCollection<Payment> Payments { get; set; }

        public RoomData()
        {
            Payments = new ObservableCollection<Payment>();
        }
    }
}
