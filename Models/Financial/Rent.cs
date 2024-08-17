/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        public FinancialTransactionItemRent Advance { get; set; } = new FinancialTransactionItemRent()
        {
            TransactionItem = "Advance",
            TransactionShareTypes = TransactionShareTypesRent.Area,
            TransactionSum = 0.0
        };


        public FinancialTransactionItemRent ColdRent { get; set; } = new FinancialTransactionItemRent() 
        {
            TransactionItem = "Cold Rent",
            TransactionShareTypes=TransactionShareTypesRent.Area,
            TransactionSum = 0.0
        };


        public bool CostsHasDataLock { get; set; } = false;


        public bool HasCredits { get; set; } = false;


        public bool HasDataLock { get; set; } = false;


        public bool HasOtherCosts { get; set; } = false;


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
                    FinancialTransactionItemRent advance
                    )
        {
            StartDate = startDate;
            ColdRent = rent;
            Advance = advance;
        }

        #endregion constructors


        // methods
        #region methods

        public void AddCredit(FinancialTransactionItemRent item)
        {
            Credits.Add(item);

            HasCredits = true;
        }


        public void AddFinacialTransactionItem(FinancialTransactionItemRent item)
        {
            Costs.Add(item);

            HasOtherCosts = true;
        }


        public void ClearCosts()
        {
            Costs.Clear();
            HasOtherCosts = false;
        }


        public void ClearCredits()
        {
            Credits.Clear();

            HasCredits = false;
        }


        public void RemoveCredit(FinancialTransactionItemRent item)
        {

            if (Credits.Count > 0 && Credits.Contains(item))
            {
                Credits.Remove(item);

                if (Credits.Count == 0)
                {
                    HasCredits = false;
                }
            }
        }


        public void RemoveFinancialTransactionItem(FinancialTransactionItemRent item)
        {
            if (Costs.Count > 0 && Costs.Contains(item))
            {
                Costs.Remove(item);

                if (Costs.Count == 0)
                {
                    HasOtherCosts = false;
                } 
            }
        }

        #endregion methods


    }
}
// EOF