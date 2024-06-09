using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharedLivingCostCalculator.ViewModels
{
    public class BillingViewModel : BaseViewModel, IRoomCostsCarrier
    {
        private readonly FlatViewModel _flatViewModel;
        public RentViewModel RentViewModel { get; set; }

        public int basedOnRent_ID => RentViewModel.ID;


        private readonly Billing _billing;
        public Billing GetBilling => _billing;
        
        // a new billing must allways create a new RentViewModel        
        //

        //
        //
        //

        public DateTime StartDate
        {
            get { return _billing.StartDate; }
            set { _billing.StartDate = value; OnPropertyChanged(nameof(StartDate)); 
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
                GetRentViewModel();


            }
        }

        public DateTime EndDate
        {
            get { return _billing.EndDate; }
            set { _billing.EndDate = value; OnPropertyChanged(nameof(EndDate));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));
                GetRentViewModel();
            }
        }

        // monthly costs
        public double TotalCostsPerPeriod
        {
            get { return _billing.TotalCostsPerPeriod; }
            set
            {
                _billing.TotalCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalCostsPerPeriod));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCostsPerPeriod)));
            }
        }

        public double TotalFixedCostsPerPeriod
        {
            get { return _billing.TotalFixedCostsPerPeriod; }
            set
            {
                _billing.TotalFixedCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalFixedCostsPerPeriod));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalFixedCostsPerPeriod)));
            }
        }

        public double TotalHeatingCostsPerPeriod
        {
            get { return _billing.TotalHeatingCostsPerPeriod; }
            set
            {
                _billing.TotalHeatingCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalHeatingCostsPerPeriod));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHeatingCostsPerPeriod)));
            }
        }

        public double TotalHeatingUnitsConsumption
        {
            get { return _billing.TotalHeatingUnitsConsumption; }
            set
            {
                _billing.TotalHeatingUnitsConsumption = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsConsumption));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHeatingUnitsConsumption)));
            }
        }

        public double TotalHeatingUnitsRoom
        {
            get { return _billing.TotalHeatingUnitsRoom; }
            set
            {
                _billing.TotalHeatingUnitsRoom = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsRoom));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHeatingUnitsRoom)));
            }
        }


        private ObservableCollection<RoomCostsViewModel> _RoomCosts;

        public event PropertyChangedEventHandler DataChange;

        public ObservableCollection<RoomCostsViewModel> RoomCosts
        {
            get { return _RoomCosts; }
            set
            {
                _RoomCosts = value;
                OnPropertyChanged(nameof(RoomCosts));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomCosts)));
            }
        }


        public BillingViewModel(FlatViewModel flatViewModel, Billing billing)
        {
            _flatViewModel = flatViewModel;
            _billing = billing;

            RoomCosts = new ObservableCollection<RoomCostsViewModel>();
            GetRentViewModel();
        }
        
        private void GetRentViewModel()
        {
            RentViewModel = _flatViewModel.GetRentForPeriod(this);
        }
        
        public void GenerateRoomCosts()
        {
            RoomCosts = new ObservableCollection<RoomCostsViewModel>();
            
            foreach (RoomViewModel room in GetFlatViewModel().Rooms)
            {
                RoomCosts.Add(new RoomCostsViewModel(room, this));
            }            
        }

        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }
    }
}
