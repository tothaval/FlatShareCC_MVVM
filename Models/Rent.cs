/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Rent 
 * 
 *  serializable data model class
 *  for RentViewModel
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace SharedLivingCostCalculator.Models
{

    [Serializable]
    public class Rent
    {

        // properties & fields
        #region properties

        [XmlIgnore]
        public double AnnualExtraCosts => ExtraCostsTotal * 12;


        [XmlIgnore]
        public double AnnualRent => ColdRent * 12;


        public double ColdRent { get; set; } = 0.0;


        public double CostsTotal => ColdRent + ExtraCostsTotal;


        public double ExtraCostsHeating { get; set; } = 0.0;


        public double ExtraCostsShared { get; set; } = 0.0;


        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;


        public bool HasCredits { get; set; } = false;


        public bool HasDataLock { get; set; } = false;


        public bool HasOtherCosts { get; set; } = false;


        public Billing? GetBilling { get; set; }
        

        public bool OtherCostsHasDataLock { get; set; } = false;


        public DateTime StartDate { get; set; } = DateTime.Now;

        #endregion properties


        // collections
        #region collections

        // storing OtherCostItems in case of other costs being factored in into rent calculation
        [XmlArray("OtherCostItemCollection")]
        public ObservableCollection<OtherCostItem> OtherCosts { get; set; } = new ObservableCollection<OtherCostItem>();


        // storing the actual rent cost shares of each room
        public ObservableCollection<RoomCosts> RoomCostShares { get; set; } = new ObservableCollection<RoomCosts>();

        #endregion collections


        // constructors
        #region constructors

        public Rent()
        {

        }


        public Rent(
                    FlatViewModel model,
                    DateTime startDate,
                    double coldRent,
                    double extraCostsShared,
                    double extraCostsHeating
                    )
        {
            StartDate = startDate;
            ColdRent = coldRent;
            ExtraCostsShared = extraCostsShared;
            ExtraCostsHeating = extraCostsHeating;

            GenerateRoomCosts(model);
        }

        #endregion constructors


        // methods
        #region methods

        public void GenerateRoomCosts(FlatViewModel flatViewModel)
        {
            RoomCostShares.Clear();

            if (flatViewModel != null)
            {
                foreach (RoomViewModel room in flatViewModel.Rooms)
                {
                    RoomCostShares.Add(
                        new RoomCosts(room)
                        );
                }
            }
        }

        #endregion methods


    }
}
// EOF