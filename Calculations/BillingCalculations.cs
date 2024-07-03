/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *    
 *  BillingCalculations 
 * 
 *  provides methods for recurring calculations related to the BillingViewModel
 *  and its associated logic
 */
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

        /// <summary>
        ///  heating units consumed by the room and an equal share of BillingViewModel SharedHeatingUnitsConsumption property
        ///  are added and the value returned
        /// </summary>
        /// <param name="billingViewModel"></param>
        /// <param name="roomConsumption"></param>
        /// <returns>a positive double value on success, -100.0 on error</returns>
        public double CombinedRoomConsumption(BillingViewModel billingViewModel, double roomConsumption)
        {
            if (SharedRoomConsumption(billingViewModel) == -100.0)
            {
                return -100.0;
            }

            return SharedRoomConsumption(billingViewModel) + roomConsumption;                
        }


        /// <summary>
        ///  heating units consumed by the room and an equal share of BillingViewModel SharedHeatingUnitsConsumption property
        ///  are added and the value divided by the total heating units consumption, the result is returned
        /// </summary>
        /// <param name="billingViewModel"></param>
        /// <param name="roomConsumption"></param>
        /// <returns>a positive double value on success, -100.0 on error</returns>
        public double CombinedRoomConsumptionRatio(BillingViewModel billingViewModel, double roomConsumption)
        {
            if (CombinedRoomConsumption(billingViewModel, roomConsumption) == -100.0)
            {
                return -100.0;
            }

            return CombinedRoomConsumption(billingViewModel, roomConsumption) / billingViewModel.TotalHeatingUnitsConsumption;
        }


        /// <summary>
        /// calculates the ratio of the consumed heating units compared to the total 
        /// consumption of heating units of a Billing period.
        /// </summary>
        /// <param name="billingViewModel"></param>
        /// <param name="roomConsumption"></param>
        /// <returns>returns roomConsumption / TotalHeatingUnitsConsumption (BillingViewModel)</returns>
        public double ConsumptionRatio(BillingViewModel billingViewModel, double roomConsumption)
        {
            return roomConsumption / billingViewModel.TotalHeatingUnitsConsumption;
        }


        /// <summary>
        /// heating units not consumed by the rooms are represented by BillingViewModel SharedHeatingUnitsConsumption property,
        /// this value is divided by the total number of rooms, the result is returned.
        /// </summary>
        /// <param name="billingViewModel"></param>
        /// <returns>a positive double value on success, -100.0 on error</returns>
        public double SharedRoomConsumption(BillingViewModel billingViewModel)
        {
            if (billingViewModel.GetFlatViewModel().RoomCount != 0)
            {
                return billingViewModel.SharedHeatingUnitsConsumption / billingViewModel.GetFlatViewModel().RoomCount;
            }

            return -100.0;
        }

        #endregion methods


    }
}
// EOF