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

        // Properties & Fields
        #region Properties & Fields

        public TransactionDurationTypes Duration { get; set; } = TransactionDurationTypes.Ongoing;


        public DateTime EndDate { get; set; }


        public DateTime StartDate { get; set; } = DateTime.Now.Date;


        public string TransactionItem { get; set; } = "other cost item";


        public TransactionShareTypesRent TransactionShareTypes { get; set; } = TransactionShareTypesRent.Equal;


        public double TransactionSum { get; set; } = 0.0;

        #endregion


        // Constructors
        #region Constructors

        public FinancialTransactionItemRent()
        {


            if (DateTime.Now.Month + 1 < 13)
            { 
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);
            }

            if (DateTime.Now.Month + 1 > 13)
            {
                EndDate = new DateTime(DateTime.Now.Year + 1, 1, 1);
            }

        }

        #endregion


    }
}
// EOF