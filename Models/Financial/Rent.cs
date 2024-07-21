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


        public FinancialTransactionItemRent ColdRent { get; set; } = new FinancialTransactionItemRent() 
        {
            TransactionItem = "Cold Rent",
            TransactionShareTypes=TransactionShareTypesRent.Area,
            TransactionSum = 0.0
        };


        public double CostsTotal => ColdRent.TransactionSum + ExtraCostsTotal;


        public FinancialTransactionItemRent FixedCostsAdvance { get; set; } = new FinancialTransactionItemRent()
        {
            TransactionItem = "Fixed",
            TransactionShareTypes = TransactionShareTypesRent.Area,
            TransactionSum = 0.0
        };


        public FinancialTransactionItemBilling HeatingCostsAdvance { get; set; } = new FinancialTransactionItemBilling()
        {
            TransactionItem = "Heating",
            TransactionShareTypes = TransactionShareTypesBilling.Consumption,
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
        [XmlArray("OtherCosts")]
        public ObservableCollection<FinancialTransactionItemRent> Costs { get; set; } = new ObservableCollection<FinancialTransactionItemRent>();


        // storing TransactionItems in case of other costs being factored in into rent calculation
        [XmlArray("Credits")]
        public ObservableCollection<FinancialTransactionItemRent> Credits { get; set; } = new ObservableCollection<FinancialTransactionItemRent>();

        #endregion collections


        // constructors
        #region constructors

        public Rent()
        {
        }


        public Rent(
                    FlatViewModel model,
                    DateTime startDate,
                    FinancialTransactionItemRent rent,
                    FinancialTransactionItemRent shared,
                    FinancialTransactionItemBilling heating
                    )
        {
            StartDate = startDate;
            ColdRent = rent;
            FixedCostsAdvance = shared;
            HeatingCostsAdvance = heating;
        }

        #endregion constructors


        // methods
        #region methods

        public void AddCredit(FinancialTransactionItemRent item)
        {
            Credits.Add(item);
        }


        public void AddFinacialTransactionItem(FinancialTransactionItemRent item)
        {
            Costs.Add(item);
        }


        public void RemoveCredit(FinancialTransactionItemRent item)
        {
            Credits.Remove(item);
        }


        public void RemoveFinancialTransactionItem(FinancialTransactionItemRent item)
        {
            Costs.Remove(item);
        }

        #endregion methods


    }
}
// EOF