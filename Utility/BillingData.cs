using SharedLivingCostCalculator.Calculations;
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
    public class BillingData
    {       
        public DateTime StartDate { get; set; } = DateTime.Now - TimeSpan.FromDays(365);

        public DateTime EndDate { get; set; } = DateTime.Now;

        public double TotalCostsPerPeriod { get; set; } = 0.0;

        public double TotalFixedCostsPerPeriod { get; set; } = 0.0;

        public double TotalHeatingCostsPerPeriod { get; set; } = 0.0;

        public double TotalHeatingUnitsConsumption { get; set; } = 0.0;

        public int basedOnRent_ID { get; set; } = -1;

       [XmlIgnore]
        public double TotalHeatingUnitsRoom { get; set; } = 0.0;

        public ObservableCollection<RoomCosts> RoomConsumptionValues { get; set; }

        public BillingData()
        {
            RoomConsumptionValues = new ObservableCollection<RoomCosts>();
                
        }
    }
}
