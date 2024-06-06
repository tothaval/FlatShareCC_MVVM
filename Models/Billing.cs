﻿using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SharedLivingCostCalculator.Models
{

    public class Billing
    {
        private readonly FlatViewModel _flatViewModel;

        // begin of billing period
        public DateTime StartDate { get; set; } = DateTime.Now - TimeSpan.FromDays(365);

        // end of billing period
        public DateTime EndDate { get; set; } = DateTime.Now;

        // combined costs of fixed costs and heating costs
        // costs need to take RoomPayments per room into consideration
        public double TotalCostsPerPeriod { get; set; } = 0.0;

        // fixed costs
        // can be calculated per room using
        // (((room area) + (shared space)/(amount of Rooms))/(total area)) * fixed costs
        public double TotalFixedCostsPerPeriod { get; set; } = 0.0;

        // heating costs 
        // shared space heating costs can be devided by the number of Rooms
        // room based heating costs must take heating units constumption into
        // account
        public double TotalHeatingCostsPerPeriod { get; set; } = 0.0;

        // heating units used in billing period
        // values for Rooms must be determined in order to
        // calculate new rent shares based on consumption
        public double TotalHeatingUnitsConsumption {  get; set; } = 0.0;

        // combined sum of room heating units consumption
        public double TotalHeatingUnitsRoom {  get; set; } = 0.0;

        // room data on heating units consumption per billing
        public ObservableCollection<RoomHeatingUnitsViewModel> RoomConsumptionValues { get; set; }

        public Billing(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;

            RoomConsumptionValues = new ObservableCollection<RoomHeatingUnitsViewModel>();

            ActivateCollection();
        }

        public Billing(FlatViewModel flatViewModel,
            DateTime startDate,
            DateTime endDate,
            double totalCostsPerPeriod,
            double totalFixedCostsPerPeriod,
            double totalHeatingCostsPerPeriod,
            double totalHeatingUnitsConsumption,
            double totalHeatingUnitsRoom)
        {
            RoomConsumptionValues = new ObservableCollection<RoomHeatingUnitsViewModel>();

            _flatViewModel = flatViewModel;

            StartDate = startDate;
            EndDate = endDate;
            TotalCostsPerPeriod = totalCostsPerPeriod;
            TotalFixedCostsPerPeriod = totalFixedCostsPerPeriod;
            TotalHeatingCostsPerPeriod = totalHeatingCostsPerPeriod;
            TotalHeatingUnitsConsumption = totalHeatingUnitsConsumption;
            TotalHeatingUnitsRoom = totalHeatingUnitsRoom;

            ActivateCollection();
        }

        private void ActivateCollection()
        {
            if (RoomConsumptionValues.Count < _flatViewModel.Rooms.Count)
            {
                RoomConsumptionValues.Clear();

                foreach (RoomViewModel room in _flatViewModel.Rooms)
                {
                    RoomConsumptionValues.Add(new RoomHeatingUnitsViewModel(new RoomHeatingUnits(room), new BillingViewModel(this)));
                }
            }
        }
    }
}
