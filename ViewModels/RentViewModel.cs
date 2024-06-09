using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels
{
    public class RentViewModel : BaseViewModel, IRoomCostsCarrier
    {
        private readonly FlatViewModel _flatViewModel;
        private readonly Rent _rent;
        public readonly BillingViewModel? BillingViewModel;
        public bool HasBilling { get; } = false;

        public Rent GetRent => _rent;

        // startdates in RentViewModel list indicate enddates of older rentviewmodels
        // the next oldest RentViewModel is the successor of a RentViewModel
        // this has to be taken into account for the cost calculations

        // a new billing must allways create a new RentViewModel
        // the user can add RentViewModels, in case a raise was received
        // for different reasons than annual billing

        public int ID
        {
            get { return _rent.ID; }
        }

        public DateTime StartDate
        {
            get { return _rent.StartDate; }
            set { _rent.StartDate = value; OnPropertyChanged(nameof(StartDate));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
            }
        }


        // monthly costs
        public double ColdRent
        {
            get { return _rent.ColdRent; }
            set { _rent.ColdRent = value; 
                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualRent));
                OnPropertyChanged(nameof(AnnualCostsTotal));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(ColdRent)));
            }

        }

        public double FixedCostsAdvance
        {
            get { return _rent.ExtraCostsShared; }
            set { _rent.ExtraCostsShared = value;
                OnPropertyChanged(nameof(FixedCostsAdvance));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(AnnualCostsTotal));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(FixedCostsAdvance)));
            }
        }

        public double HeatingCostsAdvance
        {
            get { return _rent.ExtraCostsHeating; }
            set { _rent.ExtraCostsHeating = value; 
                OnPropertyChanged(nameof(HeatingCostsAdvance));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(AnnualCostsTotal));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(HeatingCostsAdvance)));
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


        // monthly costs sums
        public double ExtraCostsTotal => FixedCostsAdvance + HeatingCostsAdvance;
        public double CostsTotal => ColdRent + ExtraCostsTotal;

        // annual interval sums
        public double AnnualRent => ColdRent * 12;
        public double AnnualExtraCosts => ExtraCostsTotal * 12;

        // annual costs sum
        public double AnnualCostsTotal => AnnualRent + AnnualExtraCosts;

        public RentViewModel(FlatViewModel flatViewModel, Rent rent, BillingViewModel? billingViewModel = null)
        {
            _flatViewModel = flatViewModel;
            _rent = rent;         

            if (billingViewModel != null)
            {
                HasBilling = true;
            }

            BillingViewModel = billingViewModel;
        }

        public void GenerateRoomCosts()
        {
            RoomCosts = new ObservableCollection<RoomCostsViewModel>();

            if (BillingViewModel != null)
            {
                RoomCosts = BillingViewModel.RoomCosts;
            }
            else
            {
                foreach (RoomViewModel room in _flatViewModel.Rooms)
                {
                    RoomCosts.Add(new RoomCostsViewModel(room, this));
                }
            }
        }

        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }
    }
}
