/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IConsumptionItem 
 * 
 *  interface for ConsumptionItem, which wraps around an FTI Billing
 *  to take in the consumed units value, which is necessary to calculate
 *  the fraction of the costs based on consumption split procedure
 */

using SharedLivingCostCalculator.Models.Financial;

namespace SharedLivingCostCalculator.Interfaces.Contract
{
    public interface IConsumptionItem
    {

        // Properties & Fields
        #region Properties & Fields

        FinancialTransactionItemBilling ConsumptionCause { get; }


        double ConsumedUnits { get; set; } 

        #endregion

    }
}
// EOF