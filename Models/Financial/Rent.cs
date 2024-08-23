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

        /// <summary>
        /// Advance represents the monthly contract costs, that are not directly tied
        /// to the area that is rented, but to the costs that accumulate over the
        /// time of the ongoing contract, like heating, water, the cleaning and
        /// maintainance of the building and so on.
        /// The advance is paid monthly to chunk those costs into smaller bits
        /// before the annual billing arrives.
        /// </summary>
        public FinancialTransactionItemRent Advance { get; set; } = new FinancialTransactionItemRent()
        {
            TransactionItem = "Advance",
            TransactionShareTypes = TransactionShareTypesRent.Area,
            TransactionSum = 0.0
        };


        /// <summary>
        /// ColdRent represents the monthly contract costs for the rented area itself,
        /// absent all other cost structures.
        /// </summary>
        public FinancialTransactionItemRent ColdRent { get; set; } = new FinancialTransactionItemRent()
        {
            TransactionItem = "Cold Rent",
            TransactionShareTypes = TransactionShareTypesRent.Area,
            TransactionSum = 0.0
        };


        /// <summary>
        /// probably obsolete, due to the fact that this was a prop 
        /// when other costs had a separate window. this prop marked
        /// the state of the datalock togglebutton of that window.
        /// </summary>
        public bool CostsHasDataLock { get; set; } = false;


        /// <summary>
        /// Value must be true if Credits
        /// count is greater than 0, meaning there are credits
        /// that have to be calculated and printed out depending on
        /// option set by the user.
        /// </summary>
        public bool HasCredits { get; set; } = false;


        /// <summary>
        /// intended to signal whether the datalock togglebutton
        /// has been checked or has not been checked by the user.
        /// </summary>
        public bool HasDataLock { get; set; } = false;


        /// <summary>
        /// Value must be true if Costs
        /// count is greater than 0, meaning there are other costs
        /// that have to be calculated and printed out depending on
        /// option set by the user.
        /// </summary>
        public bool HasOtherCosts { get; set; } = false;


        /// <summary>
        /// Value must be set to true if this is the Initial Rent.
        /// </summary>
        public bool IsInitialRent { get; set; } = false;


        /// <summary>
        /// Marks the begin of either a rent change or
        /// the rent contract if it is the initial rent.
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.Now;


        /// <summary>
        /// If true, inital monthly cost calculation
        /// will use room cost values to calculate flat cost values,
        /// else
        /// it will use flat cost values to calculate room cost values.
        /// </summary>
        public bool UseRoomCosts4InitialRent { get; set; } = false;


        /// <summary>
        /// If true, workplace logic will become activated and will
        /// be used to calculate values, as well as to change print
        /// output to show workplace based calculations.
        /// workplaces can be n per room, the split procedure is equal
        /// per room, rooms will be calculated, than the n splits per
        /// room depending on workplace count per that room.
        /// </summary>
        public bool UseWorkplaces { get; set; } = false;

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


        public Rent(bool isInitialRent = false)
        {
            IsInitialRent = isInitialRent;
        }


        public Rent(
                    FlatViewModel model,
                    DateTime startDate,
                    FinancialTransactionItemRent rent,
                    FinancialTransactionItemRent advance,
                    bool isInitialRent = false)
        {
            IsInitialRent = isInitialRent;
            StartDate = startDate;
            ColdRent = rent;
            Advance = advance;
        }

        #endregion constructors


        // methods
        #region methods

        public void AddCredit(FinancialTransactionItemRent item)
        {
            item.StartDate = StartDate;

            Credits.Add(item);

            HasCredits = true;
        }


        public void AddFinacialTransactionItem(FinancialTransactionItemRent item)
        {
            item.StartDate = StartDate;

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


        public void SetUseRoomCosts(bool useRoomCosts)
        {          
                UseRoomCosts4InitialRent = useRoomCosts;
        }

        #endregion methods


    }
}
// EOF