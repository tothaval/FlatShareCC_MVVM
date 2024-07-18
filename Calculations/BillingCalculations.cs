/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *    
 *  BillingCalculations 
 * 
 *  provides methods for recurring calculations related to the BillingViewModel
 *  and its associated logic
 */
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;

namespace SharedLivingCostCalculator.Calculations
{
    public class BillingCalculations
    {

        // constructos
        #region constructors
        
        public BillingCalculations()
        {
                
        }

        #endregion constructors


        // methods
        #region methods

        //public double HeatingUnitsConsumption()
        //{
        //    double heatingUnits = 0.0 ;

        //    foreach (ConsumptionItemViewModel item in ConsumptionItemViewModels)
        //    {
        //        if (item.ConsumptionItem.ConsumptionCause.TransactionItem.Equals(GetBilling.TotalHeatingCostsPerPeriod.TransactionItem))
        //        {
        //            heatingUnits = item.ConsumedUnits;

        //            break;
        //        }
        //    }

        //    return heatingUnits;
        //}


        //private bool TotalConsumptionIsLessThanSum()
        //{
        //    if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
        //    {
        //        double totalConsumption = ((BillingViewModel)_roomCostsCarrier).TotalHeatingUnitsConsumption;

        //        foreach (RoomCostsViewModel roomCosts in ((BillingViewModel)_roomCostsCarrier).RoomCosts)
        //        {
        //            totalConsumption -= roomCosts.HeatingUnitsConsumption;

        //            if (totalConsumption < 0)
        //            {
        //                break;
        //            }
        //        }

        //        return totalConsumption >= 0;
        //    }

        //    return false;
        //}
        #endregion methods


    }
}
// EOF