using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    class BillingPeriodViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly BillingViewModel _billingViewModel;
        public BillingViewModel BillingViewModel => _billingViewModel;
        private ValidationHelper _helper = new ValidationHelper();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool HasErrors => _helper.HasErrors;
        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public DateTime StartDate
        {
            get { return _billingViewModel.StartDate; ; }
            set
            {
                _billingViewModel.StartDate = value;
                OnPropertyChanged(nameof(StartDate));

                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));
                                    
                if (_billingViewModel.StartDate > _billingViewModel.EndDate)
                {
                    _helper.AddError("start date must be before enddate", nameof(StartDate));
                }
            }
        }


        public DateTime EndDate
        {
            get { return _billingViewModel.EndDate; ; }
            set
            {
                _billingViewModel.EndDate = value;
                OnPropertyChanged(nameof(EndDate));

                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));

                if (StartDate == EndDate || EndDate < StartDate)
                {
                    _helper.AddError("start date must be before enddate", nameof(EndDate));
                }
            }
        }

        // combined costs of fixed costs and heating costs
        // costs need to take RoomPayments per room into consideration
        public double TotalCostsPerPeriod
        {
            get { return _billingViewModel.TotalCostsPerPeriod; }
            set
            {
                _helper.ClearError(nameof(TotalCostsPerPeriod));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalCostsPerPeriod));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalCostsPerPeriod));
                }

                if (value < TotalFixedCostsPerPeriod + TotalHeatingCostsPerPeriod)
                {
                    _helper.AddError("value must be greater than combined costs", nameof(TotalCostsPerPeriod));
                }

                _billingViewModel.TotalCostsPerPeriod = value;

                OnPropertyChanged(nameof(TotalCostsPerPeriod));
            }
        }

        // fixed costs
        // can be calculated per room using
        // (((room area) + (shared space)/(amount of Rooms))/(total area)) * fixed costs
        public double TotalFixedCostsPerPeriod
        {
            get { return _billingViewModel.TotalFixedCostsPerPeriod; }
            set
            {
                _helper.ClearError(nameof(TotalCostsPerPeriod));
                _helper.ClearError(nameof(TotalFixedCostsPerPeriod));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalFixedCostsPerPeriod));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalFixedCostsPerPeriod));
                }
                        

                _billingViewModel.TotalFixedCostsPerPeriod = value;

                OnPropertyChanged(nameof(TotalFixedCostsPerPeriod));


                _billingViewModel.TotalHeatingCostsPerPeriod = TotalCostsPerPeriod - TotalFixedCostsPerPeriod;
                OnPropertyChanged(nameof(TotalHeatingCostsPerPeriod));
            }
        }

        // heating costs 
        // shared space heating costs can be devided by the number of Rooms
        // room based heating costs must take heating units constumption into
        // account
        public double TotalHeatingCostsPerPeriod
        {
            get { return _billingViewModel.TotalHeatingCostsPerPeriod; }
            set
            {
                _helper.ClearError(nameof(TotalCostsPerPeriod));
                _helper.ClearError(nameof(TotalHeatingCostsPerPeriod));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalHeatingCostsPerPeriod));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalHeatingCostsPerPeriod));
                }

                _billingViewModel.TotalHeatingCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalHeatingCostsPerPeriod));


                _billingViewModel.TotalFixedCostsPerPeriod = TotalCostsPerPeriod - TotalHeatingCostsPerPeriod;
                OnPropertyChanged(nameof(TotalFixedCostsPerPeriod));
            }
        }

        // heating units used in billing period
        // values for Rooms must be determined in order to
        // calculate new rent shares based on consumption
        public double TotalHeatingUnitsConsumption
        {
            get
            {
                return _billingViewModel.TotalHeatingUnitsConsumption;
            }
            set
            {
                _helper.ClearError(nameof(TotalHeatingUnitsConsumption));
                _helper.ClearError(nameof(TotalHeatingUnitsRoom));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalHeatingUnitsConsumption));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalHeatingUnitsConsumption));
                }


                if (value < TotalHeatingUnitsRoom)
                {
                    _helper.AddError("value can not be lesser than combined rooms value", nameof(TotalHeatingUnitsConsumption));
                }

                _billingViewModel.TotalHeatingUnitsConsumption = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsConsumption));
            }
        }

        public double TotalHeatingUnitsRoom
        {
            get
            {
                return _billingViewModel.TotalHeatingUnitsRoom;
            }
            set
            {
                _helper.ClearError(nameof(TotalHeatingUnitsRoom));
                _helper.ClearError(nameof(TotalHeatingUnitsConsumption));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalHeatingUnitsRoom));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalHeatingUnitsRoom));
                }

                if (value > TotalHeatingUnitsConsumption)
                {
                    _helper.AddError("value can not be greater than total value", nameof(TotalHeatingUnitsRoom));
                }

                _billingViewModel.TotalHeatingUnitsRoom = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsRoom));
            }
        }


        public ObservableCollection<RoomHeatingUnitsViewModel> RoomConsumptionValues => _billingViewModel.RoomConsumptionValues;


        public void CalculateRoomsConsumption()
        {
            if (RoomConsumptionValues != null)
            {
                TotalHeatingUnitsRoom = 0.0;

                foreach (RoomHeatingUnitsViewModel roomConsumption in RoomConsumptionValues)
                {
                    TotalHeatingUnitsRoom += roomConsumption.HeatingUnitsConsumption;
                }

                OnPropertyChanged(nameof(TotalHeatingUnitsRoom));
            }
        }

        public BillingPeriodViewModel(BillingViewModel billingViewModel)
        {
            _billingViewModel = billingViewModel;

            if (_billingViewModel == null)
            {
                _billingViewModel = new BillingViewModel(new Billing(new FlatViewModel(new Flat())));
            }


            _helper.ErrorsChanged += (_, e) =>
            {
                OnPropertyChanged(nameof(_helper));
                this.ErrorsChanged?.Invoke(this, e);
            };

                foreach (RoomHeatingUnitsViewModel rhu in RoomConsumptionValues)
                {
                    rhu.HeatingUnitsChange += HeatingUnitsChange;
                }

        }

        private void HeatingUnitsChange(object? sender, EventArgs e)
        {
            CalculateRoomsConsumption();
        }

    }
}
