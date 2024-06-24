using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Calculations
{
    public static class BillingCalculations
    {


        /// <summary>
        ///  heating units consumed by the room and an equal share of BillingViewModel SharedHeatingUnitsConsumption property
        ///  are added and the value returned
        /// </summary>
        /// <param name="billingViewModel"></param>
        /// <param name="roomConsumption"></param>
        /// <returns>a positive double value on success, -100.0 on error</returns>
        public static double CombinedRoomConsumption(BillingViewModel billingViewModel, double roomConsumption)
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
        public static double CombinedRoomConsumptionRatio(BillingViewModel billingViewModel, double roomConsumption)
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
        public static double ConsumptionRatio(BillingViewModel billingViewModel, double roomConsumption)
        {
            return roomConsumption / billingViewModel.TotalHeatingUnitsConsumption;
        }


        /// <summary>
        /// heating units not consumed by the rooms are represented by BillingViewModel SharedHeatingUnitsConsumption property,
        /// this value is divided by the total number of rooms, the result is returned.
        /// </summary>
        /// <param name="billingViewModel"></param>
        /// <returns>a positive double value on success, -100.0 on error</returns>
        public static double SharedRoomConsumption(BillingViewModel billingViewModel)
        {
            if (billingViewModel.GetFlatViewModel().RoomCount != 0)
            {
                return billingViewModel.SharedHeatingUnitsConsumption / billingViewModel.GetFlatViewModel().RoomCount;
            }

            return -100.0;
        }
    }
}
