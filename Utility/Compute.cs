using PropertyTools.Wpf;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        /// <summary>
        /// refactor into subclasses, comment this summary
        /// </summary>
        /// <param name="flatViewModel"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public ObservableCollection<RentViewModel> FindRelevantRentViewModels(FlatViewModel flatViewModel, int year)            
        {
            ObservableCollection<RentViewModel> preSortList = new ObservableCollection<RentViewModel>();
            ObservableCollection<RentViewModel> RentList = new ObservableCollection<RentViewModel>();

            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31);

            int counter = 0;

            // rent begins before selected year ends
            if (flatViewModel.InitialRent.StartDate < endDate)
            {
                counter++;
            }

            // rent begins after Billing period start but before Billing period end
            if (flatViewModel.InitialRent.StartDate > startDate || flatViewModel.InitialRent.StartDate < endDate)
            {
                counter++;
            }

            if (counter > 0)
            {
                preSortList.Add(flatViewModel.InitialRent);
            }

            if (flatViewModel.RentUpdates.Count > 0)
            {
                // filling the collection with potential matches
                foreach (RentViewModel rent in flatViewModel.RentUpdates)
                {
                    // rent begins after selected year ends
                    if (rent.StartDate.Year > year)
                    {
                        continue;
                    }

                    // rent begins before selected year starts
                    if (rent.StartDate < startDate)
                    {
                        preSortList.Add(rent);
                        continue;
                    }

                    // rent begins before selected year ends
                    if (rent.StartDate < endDate)
                    {
                        preSortList.Add(rent);

                        continue;
                    }

                    // rent begins after Billing period start but before Billing period end
                    if (rent.StartDate >= startDate && rent.StartDate <= endDate)
                    {
                        preSortList.Add(rent);
                    }
                }
            }

            if (preSortList.Count > 1)
            {
                preSortList = new ObservableCollection<RentViewModel>(preSortList.OrderBy(i => i.StartDate));
            }

            RentViewModel? comparer = null;
            bool firstRun = true;
            bool beginsWithYear = false;

            // building a collection of relevant rent items
            foreach (RentViewModel item in preSortList)
            {
                if (firstRun)
                {
                    firstRun = false;
                    comparer = item;
                    continue;
                }

                if (item.StartDate >= startDate)
                {
                    RentList.Add(item);

                    if (item.StartDate == startDate)
                    {
                        beginsWithYear = true;
                    }
                    continue;
                }

                if (item.StartDate < startDate && item.StartDate > comparer.StartDate && !beginsWithYear)
                {
                    comparer = item;
                }
            }


            if (comparer != null)
            {
                RentList.Add(comparer);
            }

            if (beginsWithYear)
            {
                int i = 0;

                while (true)
                {
                    if (RentList.Count > i)
                    {
                        if (RentList[i].StartDate.Year < year)
                        {
                            RentList.RemoveAt(i);

                            i--;
                        }

                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // sort List by StartDate, ascending
            if (RentList.Count > 1)
            {
                RentList = new ObservableCollection<RentViewModel>(RentList.OrderBy(i => i.StartDate));
            }

            return RentList;
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


        public BillingViewModel? SearchForBillingViewModel(FlatViewModel flatViewModel, int year)
        {
            foreach (BillingViewModel billingViewModel in flatViewModel.AnnualBillings)
            {
                if (billingViewModel.Year ==year)
                {
                    return billingViewModel;
                }
            }

            return null;
        }

        public BillingViewModel? SearchForMostRecentBillingViewModel(FlatViewModel flatViewModel)
        {

            ObservableCollection<BillingViewModel> annualBillings = flatViewModel.AnnualBillings;

            annualBillings = new ObservableCollection<BillingViewModel>(annualBillings.OrderBy(i => i.Year));

            if (annualBillings.Count > 0)
            {
                return annualBillings.Last();
            }

            return null;
        }


        public RentViewModel? SearchForMostRecentRentViewModel(FlatViewModel flatViewModel)
        {

            ObservableCollection<RentViewModel> rentChanges = flatViewModel.RentUpdates;

            rentChanges = new ObservableCollection<RentViewModel>(rentChanges.OrderBy(i => i.StartDate));

            if (rentChanges.Count > 0)
            {
                return rentChanges.Last();
            }

            return null;
        }

    }
}
