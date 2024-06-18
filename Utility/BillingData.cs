/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingData 
 * 
 *  helper class to serialize BillingViewModel
 *
 *  obsolete, Billing now holds a parameterless constructor
 *  
 *  to do: remove and replace with Billing model
 */
using SharedLivingCostCalculator.Models;
using System.Collections.ObjectModel;
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


       [XmlIgnore]
        public double TotalHeatingUnitsRoom { get; set; } = 0.0;


        public ObservableCollection<RoomCosts> RoomConsumptionValues { get; set; }


        public BillingData()
        {
            RoomConsumptionValues = new ObservableCollection<RoomCosts>();
                
        }


    }
}
// EOF