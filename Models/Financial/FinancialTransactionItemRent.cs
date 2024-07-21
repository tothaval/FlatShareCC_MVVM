/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FinancialTransactionItemRent 
 * 
 *  serializable data model class
 *  for FinancialTransactionItemViewModel
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;

namespace SharedLivingCostCalculator.Models.Financial
{
    public class FinancialTransactionItemRent : IFinancialTransactionItem
    {

        // properties & fields
        #region properties

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