/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
        public double TransactionSum { get; set; }


        public TransactionShareTypes TransactionShareTypes { get; set; }


        public string TransactionItem { get; set; }
    }
}
// EOF