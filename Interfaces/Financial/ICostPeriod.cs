/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ICostPeriod 
 * 
 *  interface for a cost collection over a period of time, mostly for billing logic.
 */

namespace SharedLivingCostCalculator.Interfaces.Financial
{
    public interface ICostPeriod
    {

        // properties & fields
        #region properties & fields

        public int NumberOfDays { get; }


        public int NumberOfRequiredPayments { get; set; }


        public DateTime PeriodBegin { get; set; }


        public DateTime PeriodEnd { get; set; }

        #endregion properties & fields


        // collections
        #region collections

        public ICollection<ICostItem> CostItems { get; set; }

        #endregion collections


    }
}
// EOF