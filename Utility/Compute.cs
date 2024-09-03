using PropertyTools.Wpf;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels;
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


        public double CostsFactor(double costs, double totalCosts)
        {
            if (totalCosts != null)
            {
                return costs / totalCosts;
            }

            return -2;
        }


        public DateTime DateEvaluation(DateTime date, FlatViewModel flatViewModel)
        {
            DateTime contractStart = new DateTime(
                flatViewModel.InitialRent.StartDate.Year,
                flatViewModel.InitialRent.StartDate.Month,
                flatViewModel.InitialRent.StartDate.Day);

            if (date < contractStart)
            {
                date = contractStart;
            }

            return date;
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


        public bool PrintThisRoom(PrintViewModel printViewModel, RoomViewModel compareToThisRoom)
        {
            if (printViewModel.PrintAllSelected || printViewModel.PrintRoomsSelected)
            {
                return true;
            }

            if (printViewModel.SelectedRoom != null)
            {
                bool printThisRoom = printViewModel.PrintExcerptSelected &&
                    printViewModel.SelectedRoom.RoomName.Equals(compareToThisRoom.RoomName) &&
                    printViewModel.SelectedRoom.RoomArea == compareToThisRoom.RoomArea;

                return printThisRoom;
            }

            return false;
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
    }
}
