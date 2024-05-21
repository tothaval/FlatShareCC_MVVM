<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
=======
﻿using SharedLivingCostCalculator.BoilerPlateReduction;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
>>>>>>> Stashed changes
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    internal class BillingPeriod : INotifyDataErrorInfo
    {
<<<<<<< Updated upstream
        public DateTime BillingPeriodStartDate { get; set; }
        public DateTime BillingPeriodEndDate { get; set; }

        public double TotalCostsPerPeriod { get; set; }
        public double TotalFixedCostsPerPeriod { get; set;}
        public double TotalHeatingCostsPerPeriod { get; set; }

        // additional fields for room heating units consumption
        // a separate class for payments on a per room basis
=======
        private ValidationHelper helper;
        public ValidationHelper Helper { get; }

        private readonly FlatViewModel _flatViewModel;

        // calculate costs per room for annual billing
        // if heating units consumption is > 0 heating costs
        // must be calculated based on consumption, not on area share
        // all other costs can be calculated based on area share

        private DateTime startDate;

        public DateTime StartDate
        {
            get { return startDate; }
            set {

                helper.ClearError();
                if (startDate.Year > endDate.Year)
                {
                    helper.AddError("start date must be before enddate", nameof(StartDate));
                }

                startDate = value;
            }
        }

        private DateTime endDate;

        public DateTime EndDate
        {
            get { return endDate; }
            set {

                helper.ClearError();
                if (StartDate.Date == EndDate.Date || EndDate.Year < StartDate.Year)
                {
                    helper.AddError("start date must be before enddate", nameof(EndDate));
                }
                endDate = value;
            }
        }

        // combined costs of fixed costs and heating costs
        // costs need to take payments per room into consideration
        private double totalCostsPerPeriod;

        public double TotalCostsPerPeriod
        {
            get { return totalCostsPerPeriod; }
            set
            {
                helper.ClearError();

                if (Double.IsNaN(value))
                {
                    helper.AddError("value must be a number", nameof(TotalCostsPerPeriod));
                }

                if (value < 0)
                {
                    helper.AddError("value must be greater than 0", nameof(TotalCostsPerPeriod));
                }
                totalCostsPerPeriod = value; }
        }

        // fixed costs
        // can be calculated per room using
        // (((room area) + (shared space)/(amount of rooms))/(total area)) * fixed costs
        private double totalFixedCostsPerPeriod;

        public double TotalFixedCostsPerPeriod
        {
            get { return totalFixedCostsPerPeriod; }
            set
            {
                helper.ClearError();

                if (Double.IsNaN(value))
                {
                    helper.AddError("value must be a number", nameof(TotalFixedCostsPerPeriod));
                }

                if (value < 0)
                {
                    helper.AddError("value must be greater than 0", nameof(TotalFixedCostsPerPeriod));
                }
                totalFixedCostsPerPeriod = value; }
        }
        // heating costs 
        // shared space heating costs can be devided by the number of rooms
        // room based heating costs must take heating units constumption into
        // account
        private double totalHeatingCostsPerPeriod;

        public double TotalHeatingCostsPerPeriod
        {
            get { return totalHeatingCostsPerPeriod; }
            set
            {
                helper.ClearError();

                if (Double.IsNaN(value))
                {
                    helper.AddError("value must be a number", nameof(TotalHeatingCostsPerPeriod));
                }

                if (value < 0)
                {
                    helper.AddError("value must be greater than 0", nameof(TotalHeatingCostsPerPeriod));
                }
                totalHeatingCostsPerPeriod = value; }
        }
        // heating units used in billing period
        // values for rooms must be determined in order to
        // calculate new rent shares based on consumption
        private double totalHeatingUnitsConsumption;

        public double TotalHeatingUnitsConsumption
    {
            get { return totalHeatingUnitsConsumption; }
            set
            {
                helper.ClearError();

                if (Double.IsNaN(value))
                {
                    helper.AddError("value must be a number", nameof(TotalHeatingUnitsConsumption));
                }

                if (value < 0)
                {
                    helper.AddError("value must be greater than 0", nameof(TotalHeatingUnitsConsumption));
                }
                totalHeatingUnitsConsumption = value; }
        }
        //public BillingPeriod(DateTime startDate, DateTime endDate)
        //{
        //    StartDate = startDate;
        //    EndDate = endDate;
        //}

        public BillingPeriod(ValidationHelper validationHelper)
        {
            helper = validationHelper;

            Helper.ErrorsChanged += (_, e) =>
            {
                OnPropertyChanged(nameof(Helper));
                this.ErrorsChanged?.Invoke(this, e);
            };
                
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<RoomHeatingUnitsConsumption> RoomConsumptionValues { get; set; }

        // additional fields for room heating units consumption
        // a separate class for payments on a per room basis

        public BillingPeriod(FlatViewModel flatViewModel, ValidationHelper validationHelper)
        {
            helper = validationHelper;

            RoomConsumptionValues = new ObservableCollection<RoomHeatingUnitsConsumption>();

            foreach (Room room in flatViewModel.rooms)
            {
                RoomConsumptionValues.Add(
                    new RoomHeatingUnitsConsumption(room));
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool HasErrors => helper.HasErrors;
        public IEnumerable GetErrors(string? propertyName) => helper.GetErrors(propertyName);

>>>>>>> Stashed changes
    }
}
