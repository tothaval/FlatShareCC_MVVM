/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Rent 
 * 
 *  serializable data model class
 *  for RentViewModel
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace SharedLivingCostCalculator.Models.Financial
{

    [Serializable]
    public class Rent
    {

        // properties & fields
        #region properties

        [XmlIgnore]
        public double AnnualExtraCosts => ExtraCostsTotal * 12;


        [XmlIgnore]
        public double AnnualRent => ColdRent.Cost * 12;


        public CostItem ColdRent { get; set; } = new CostItem();


        public double CostsTotal => ColdRent.Cost + ExtraCostsTotal;


        public CostItem ExtraCostsHeating { get; set; } = new CostItem();


        public CostItem ExtraCostsShared { get; set; } = new CostItem();


        public double ExtraCostsTotal => ExtraCostsShared.Cost + ExtraCostsHeating.Cost;


        public bool HasCredits { get; set; } = false;


        public bool HasDataLock { get; set; } = false;


        public Billing? GetBilling { get; set; }


        public bool CostsHasDataLock { get; set; } = false;


        public DateTime StartDate { get; set; } = DateTime.Now;

        #endregion properties


        // collections
        #region collections

        // storing CostItems in case of other costs being factored in into rent calculation
        [XmlArray("OtherCostItemCollection")]
        public ObservableCollection<CostItem> Costs { get; set; } = new ObservableCollection<CostItem>();


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
                    CostItem rent,
                    CostItem shared,
                    CostItem heating
                    )
        {
            StartDate = startDate;
            ColdRent = rent;
            ExtraCostsShared = shared;
            ExtraCostsHeating = heating;

            GenerateCosts();

            GenerateRoomCosts(model);
        }

        #endregion constructors


        // methods
        #region methods

        public void GenerateCosts()
        {
            Costs.Clear();

            Costs.Add(ColdRent);
            Costs.Add(ExtraCostsShared);
            Costs.Add(ExtraCostsHeating);
        }



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