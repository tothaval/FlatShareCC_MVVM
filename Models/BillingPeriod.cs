using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SharedLivingCostCalculator.Models
{
    internal class BillingPeriod
    {
        private readonly FlatViewModel _flatViewModel;

        // calculate costs per room for annual billing
        // if heating units consumption is > 0 heating costs
        // must be calculated based on consumption, not on area share
        // all other costs can be calculated based on area share

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
                
        // combined costs of fixed costs and heating costs
        // costs need to take payments per room into consideration
        public double TotalCostsPerPeriod { get; set; }

        // fixed costs
        // can be calculated per room using
        // (((room area) + (shared space)/(amount of rooms))/(total area)) * fixed costs
        public double TotalFixedCostsPerPeriod { get; set;}

        // heating costs 
        // shared space heating costs can be devided by the number of rooms
        // room based heating costs must take heating units constumption into
        // account
        public double TotalHeatingCostsPerPeriod { get; set; }
        
        // heating units used in billing period
        // values for rooms must be determined in order to
        // calculate new rent shares based on consumption
        public double TotalHeatingUnitsConsumption {  get; set; }
        //public BillingPeriod(DateTime startDate, DateTime endDate)
        //{
        //    StartDate = startDate;
        //    EndDate = endDate;
        //}

        public ObservableCollection<RoomHeatingUnitsConsumption> RoomConsumptionValues { get; set; }

        // additional fields for room heating units consumption
        // a separate class for payments on a per room basis

        public BillingPeriod(FlatViewModel flatViewModel)
        {
            RoomConsumptionValues = new ObservableCollection<RoomHeatingUnitsConsumption>();

            foreach (Room room in flatViewModel.rooms)
            {
                RoomConsumptionValues.Add(
                    new RoomHeatingUnitsConsumption(room));
            }
        }
    }
}
