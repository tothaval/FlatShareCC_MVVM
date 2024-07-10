/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IFinancialTransactionPeriod 
 * 
 *  interface for a collection of financial transactions over a period of time,
 *  mostly for billing logic.
 */

using PropertyTools;
using System.Collections.ObjectModel;

namespace SharedLivingCostCalculator.Interfaces.Financial
{
    public interface IFinancialTransactionPeriod
    {

        // properties & fields
        #region properties & fields

        public int NumberOfDays { get; }


        public int NumberOfTransactions { get; set; }


        public DateTime PeriodBegin { get; set; }


        public DateTime PeriodEnd { get; set; }

        #endregion properties & fields


        // collections
        #region collections

        public ObservableCollection<IFinancialTransactionItem> TransactionItems { get; set; }

        #endregion collections


    }
}
// EOF