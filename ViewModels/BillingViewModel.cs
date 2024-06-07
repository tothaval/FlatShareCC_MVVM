using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharedLivingCostCalculator.ViewModels
{
    public class BillingViewModel : BaseViewModel
    {
        private readonly Billing _billing;
        public Billing GetBilling => _billing;
        
        // a new billing must allways create a new RentViewModel
        //
        //

        //
        //
        //

        public DateTime StartDate
        {
            get { return _billing.StartDate; }
            set { _billing.StartDate = value; OnPropertyChanged(nameof(StartDate)); }
        }

        public DateTime EndDate
        {
            get { return _billing.EndDate; }
            set { _billing.EndDate = value; OnPropertyChanged(nameof(EndDate)); }
        }

        // monthly costs
        public double TotalCostsPerPeriod
        {
            get { return _billing.TotalCostsPerPeriod; }
            set
            {
                _billing.TotalCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalCostsPerPeriod));
            }
        }

        public double TotalFixedCostsPerPeriod
        {
            get { return _billing.TotalFixedCostsPerPeriod; }
            set
            {
                _billing.TotalFixedCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalFixedCostsPerPeriod));
            }
        }

        public double TotalHeatingCostsPerPeriod
        {
            get { return _billing.TotalHeatingCostsPerPeriod; }
            set
            {
                _billing.TotalHeatingCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalHeatingCostsPerPeriod));
            }
        }

        public double TotalHeatingUnitsConsumption
        {
            get { return _billing.TotalHeatingUnitsConsumption; }
            set
            {
                _billing.TotalHeatingUnitsConsumption = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsConsumption));
            }
        }

        public double TotalHeatingUnitsRoom
        {
            get { return _billing.TotalHeatingUnitsRoom; }
            set
            {
                _billing.TotalHeatingUnitsRoom = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsRoom));
            }
        }


        public ObservableCollection<RoomHeatingUnitsViewModel> RoomConsumptionValues
        {
            get { return _billing.RoomConsumptionValues; }
            set
            {
                _billing.RoomConsumptionValues = value;
                OnPropertyChanged(nameof(RoomConsumptionValues));
            }
        }


        public BillingViewModel(Billing billing)
        {
            _billing = billing;                        
        }
    }
}
