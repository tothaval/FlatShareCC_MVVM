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

        // Properties & Fields
        #region Properties & Fields
        
        public string TransactionItem { get; set; } = "other cost item";


        public TransactionShareTypesBilling TransactionShareTypes { get; set; } = TransactionShareTypesBilling.Equal;


        public double TransactionSum { get; set; } = 0.0;

        #endregion properties


        // Constructors
        #region Constructors

        public FinancialTransactionItemBilling()
        {

        }

        #endregion


    }
}
// EOF