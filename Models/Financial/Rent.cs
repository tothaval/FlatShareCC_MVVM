/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Rent 
 * 
 *  serializable data model class
 *  for RentViewModel
 */
using SharedLivingCostCalculator.Enums;
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
        public double AnnualRent => ColdRent.TransactionSum * 12;


        public FinancialTransactionItem ColdRent { get; set; } = new FinancialTransactionItem() 
        {
            TransactionItem = "Cold Rent",
            TransactionShareTypes=TransactionShareTypes.Area,
            TransactionSum = 0.0
        };


        public double CostsTotal => ColdRent.TransactionSum + ExtraCostsTotal;


        public FinancialTransactionItem FixedCostsAdvance { get; set; } = new FinancialTransactionItem()
        {
            TransactionItem = "Advance Fixed",
            TransactionShareTypes = TransactionShareTypes.Area,
            TransactionSum = 0.0
        };


        public FinancialTransactionItem HeatingCostsAdvance { get; set; } = new FinancialTransactionItem()
        {
            TransactionItem = "Advance Heating",
            TransactionShareTypes = TransactionShareTypes.Consumption,
            TransactionSum = 0.0
        };


        public double ExtraCostsTotal => FixedCostsAdvance.TransactionSum + HeatingCostsAdvance.TransactionSum;


        public bool HasCredits { get; set; } = false;


        public bool HasDataLock { get; set; } = false;


        public Billing? GetBilling { get; set; }


        public bool CostsHasDataLock { get; set; } = false;


        public DateTime StartDate { get; set; } = DateTime.Now;

        #endregion properties


        // collections
        #region collections

        // storing TransactionItems in case of other costs being factored in into rent calculation
        [XmlArray("OtherCostItemCollection")]
        public ObservableCollection<FinancialTransactionItem> Costs { get; set; } = new ObservableCollection<FinancialTransactionItem>();


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
                    FinancialTransactionItem rent,
                    FinancialTransactionItem shared,
                    FinancialTransactionItem heating
                    )
        {
            StartDate = startDate;
            ColdRent = rent;
            FixedCostsAdvance = shared;
            HeatingCostsAdvance = heating;

            GenerateRoomCosts(model);
        }

        #endregion constructors


        // methods
        #region methods

        public void AddCostItem(FinancialTransactionItem item)
        {
            Costs.Add(item);
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


        public void RemoveCostItem(FinancialTransactionItem item)
        {
            Costs.Remove(item);
        }

        #endregion methods


    }
}
// EOF