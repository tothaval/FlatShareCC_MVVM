/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Billing 
 * 
 *  data model class for BillingViewModel
 */

using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class Billing
    {

        // properties & fields
        #region properties

        // end of billing period
        public DateTime EndDate { get; set; } = DateTime.Now;


        public bool HasCredit { get; set; } = false;


        public bool HasDataLock { get; set; } = false;


        public bool HasPayments { get; set; } = false;


        // begin of billing period
        public DateTime StartDate { get; set; } = DateTime.Now - TimeSpan.FromDays(365);


        // combined costs of fixed costs and heating costs
        // costs need to take RoomPayments per room into consideration
        public double TotalCostsPerPeriod { get; set; } = 0.0;


        // fixed costs
        // can be calculated per room using
        // (((room area) + (shared space)/(amount of Rooms))/(total area)) * fixed costs
        public double TotalFixedCostsPerPeriod { get; set; } = 0.0;


        // heating costs 
        // shared space heating costs can be devided by the number of Rooms
        // room based heating costs must take heating units constumption into
        // account
        public double TotalHeatingCostsPerPeriod { get; set; } = 0.0;


        // heating units used in billing period
        // values for Rooms must be determined in order to
        // calculate new rent shares based on consumption
        public double TotalHeatingUnitsConsumption {  get; set; } = 0.0;


        // combined sum of room heating units consumption
        public double TotalHeatingUnitsRoom {  get; set; } = 0.0;

        #endregion properties


        // collections
        #region collections

        // storing the costs of each room
        // per billing period and the consumption of heating units per billing period
        [XmlArray("HeatingUnits")]
        public ObservableCollection<RoomCosts> RoomCostsConsumptionValues { get; set; } = new ObservableCollection<RoomCosts>();


        // storing the payments of each room
        // per billing period
        [XmlArray("Payments")]
        public ObservableCollection<RoomPayments> RoomPayments { get; set; } = new ObservableCollection<RoomPayments>();

        #endregion collections


        // constructors
        #region constructors

        public Billing()
        {
        }


        public Billing(
                FlatViewModel model
                )
        {

            foreach (RoomViewModel room in model.Rooms)
            {
                RoomCostsConsumptionValues.Add(new RoomCosts(room));
                RoomPayments.Add(new RoomPayments(room));
            }
        }


        public Billing(
                        FlatViewModel model,
                        DateTime startDate,
                        DateTime endDate,
                        double totalCostsPerPeriod,
                        double totalFixedCostsPerPeriod,
                        double totalHeatingCostsPerPeriod,
                        double totalHeatingUnitsConsumption,
                        double totalHeatingUnitsRoom
                        )
        {
            StartDate = startDate;
            EndDate = endDate;
            TotalCostsPerPeriod = totalCostsPerPeriod;
            TotalFixedCostsPerPeriod = totalFixedCostsPerPeriod;
            TotalHeatingCostsPerPeriod = totalHeatingCostsPerPeriod;
            TotalHeatingUnitsConsumption = totalHeatingUnitsConsumption;
            TotalHeatingUnitsRoom = totalHeatingUnitsRoom;

            foreach (RoomViewModel room in model.Rooms)
            {
                RoomCostsConsumptionValues.Add(new RoomCosts(room));
                RoomPayments.Add(new RoomPayments(room));
            }
        }
        
        #endregion constructors


    }
}
// EOF