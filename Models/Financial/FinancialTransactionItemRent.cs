/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FinancialTransactionItemRent 
 * 
 *  serializable data model class
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;

namespace SharedLivingCostCalculator.Models.Financial
{
    public class FinancialTransactionItemRent : IFinancialTransactionItem
    {

        // properties & fields
        #region properties


        public TransactionDurationTypes Duration { get; set; } = TransactionDurationTypes.Ongoing;


        public int Rates { get; set; } = 1;


        public DateTime StartDate { get; set; } = DateTime.Now.Date;


        public double TransactionSum { get; set; } = 0.0;


        public TransactionShareTypesRent TransactionShareTypes { get; set; } = TransactionShareTypesRent.Equal;


        public string TransactionItem { get; set; } = "other cost item";

        #endregion properties


        // constructors
        #region constructors

        public FinancialTransactionItemRent()
        {

        }

        #endregion constructors


    }
}
// EOF