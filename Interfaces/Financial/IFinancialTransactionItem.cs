/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IFinancialTransactionItem 
 * 
 *  interface for costitem feature
 */
using SharedLivingCostCalculator.Enums;

namespace SharedLivingCostCalculator.Interfaces.Financial
{
    public interface IFinancialTransactionItem
    {

        // Properties & Fields
        #region Properties & Fields

        public double TransactionSum { get; set; }


        public string TransactionItem { get; set; } 

        #endregion


    }
}
// EOF