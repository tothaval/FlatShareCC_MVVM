using PropertyTools.Wpf;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Utility
{
    public class Compute
    {
        public Compute()
        {

        }


        public double GetAreaSharedCostsShare(Room room, RentViewModel rentViewModel)
        {
            double areaShareRatio = new Compute().RentedAreaShareRatio(room, rentViewModel.GetFlatViewModel());

            double areaSharedCostsShare = areaShareRatio * rentViewModel.GetFTIShareSum(Enums.TransactionShareTypesRent.Area);

            return areaSharedCostsShare;
        }


        public double GetEqualSharedCostShare(RentViewModel rentViewModel)
        {
            double equalRatio = rentViewModel.GetFTIShareSum(Enums.TransactionShareTypesRent.Equal) / rentViewModel.GetFlatViewModel().RoomCount;

            return equalRatio;
        }


        public double RentedAreaShare(Room room, FlatViewModel flatViewModel)
        {
            double sharedAreaShare = flatViewModel.SharedArea / flatViewModel.RoomCount;

            double rentedArea = room.RoomArea + sharedAreaShare;

            return rentedArea;
        }

        public double RentedAreaShareRatio(Room room, FlatViewModel flatViewModel)
        {
            double rentedArea = RentedAreaShare(room, flatViewModel);

            return rentedArea / flatViewModel.Area;
        }


        public double RoomAdvanceShare(BillingViewModel billingViewModel, RentViewModel rentViewModel, Room room)
        {
            double fixedCostsFactor = billingViewModel.TotalFixedCostsPerPeriod / billingViewModel.TotalCostsPerPeriod;
            double heatingCostsFactor = billingViewModel.TotalHeatingCostsPerPeriod / billingViewModel.TotalCostsPerPeriod;

            double roomConsumptionPercentage =
                billingViewModel.GetRoomConsumptionPercentage(
                    room
                    );

            double advanceShare = fixedCostsFactor * rentViewModel.Advance * RentedAreaShareRatio(room, rentViewModel.GetFlatViewModel())
                + heatingCostsFactor * rentViewModel.Advance * roomConsumptionPercentage;

            return advanceShare;
        }
    }
}
