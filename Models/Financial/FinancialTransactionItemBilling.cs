/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FinancialTransactionItemBilling 
 * 
 *  serializable data model class
 *  for FinancialTransactionItemViewModel
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;

namespace SharedLivingCostCalculator.Models.Financial
{
    public class FinancialTransactionItemBilling : IFinancialTransactionItem
    {

        // properties & fields
        #region properties

        public double TransactionSum { get; set; } = 0.0;


        public TransactionShareTypesBilling TransactionShareTypes { get; set; } = TransactionShareTypesBilling.Equal;


        public string TransactionItem { get; set; } = "other cost item";

        #endregion properties


        // constructors
        #region constructors

        public FinancialTransactionItemBilling()
        {

        }

        #endregion constructors


    }
}
// EOF