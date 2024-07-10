/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Flat 
 * 
 *  serializable data model class
 *  for CostItemViewModel
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;

namespace SharedLivingCostCalculator.Models.Financial
{
    [Serializable]
    public class FinancialTransactionItem : IFinancialTransactionItem
    {

        // properties & fields
        #region properties

        public double TransactionSum { get; set; } = 0.0;


        public TransactionShareTypes TransactionShareTypes { get; set; } = TransactionShareTypes.Equal;


        public string TransactionItem { get; set; } = "other cost item";

        #endregion properties


        // constructors
        #region constructors

        public FinancialTransactionItem()
        {

        }

        #endregion constructors


    }
}

// EOF