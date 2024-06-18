/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Rent 
 * 
 *  serializable data model class
 *  for RentViewModel
 */
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace SharedLivingCostCalculator.Models
{

    [Serializable]
    public class Rent
    {

        public int ID {  get; set; } = 0;


        public Billing? GetBilling { get; set; }


        public DateTime StartDate { get; set; } = DateTime.Now;


        public double ColdRent { get; set; } = 0.0;


        [XmlIgnore]
        public double AnnualRent => ColdRent * 12;


        public double ExtraCostsShared { get; set; } = 0.0;


        public double ExtraCostsHeating { get; set; } = 0.0;


        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;


        public double CostsTotal => ColdRent + ExtraCostsTotal;


        [XmlIgnore]
        public double AnnualExtraCosts => ExtraCostsTotal * 12;


        public bool HasOtherCosts { get; set; } = false;


        public bool HasDataLock { get; set; } = false;


        // storing the actual rent cost shares of each room
        public ObservableCollection<RoomCosts> RoomCostShares { get; set; } = new ObservableCollection<RoomCosts>();


        public Rent()
        {

        }


        public Rent(
                    FlatViewModel model, 
                    int id,
                    DateTime startDate,
                    double coldRent,
                    double extraCostsShared,
                    double extraCostsHeating
                    )
        {
            ID = id;
            StartDate = startDate;
            ColdRent = coldRent;
            ExtraCostsShared = extraCostsShared;
            ExtraCostsHeating = extraCostsHeating;

            foreach (RoomViewModel room in model.Rooms)
            {
                RoomCostShares.Add(
                    new RoomCosts(room)
                    );
            }

        }


    }
}
// EOF