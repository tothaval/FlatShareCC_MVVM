/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentViewModel  : BaseViewModel
 * 
 *  viewmodel for Rent model
 */
using SharedLivingCostCalculator.Interfaces;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class RentViewModel : BaseViewModel, IRoomCostsCarrier
    {

        private readonly FlatViewModel _flatViewModel;


        private Rent _Rent;
        public Rent Rent
        {
            get { return _Rent; }
            set
            {
                _Rent = value;
                OnPropertyChanged(nameof(Rent));
            }
        }


        private BillingViewModel? _BillingViewModel;
        public BillingViewModel? BillingViewModel
        {
            get { return _BillingViewModel; }
            set
            {
                _BillingViewModel = value;

                if (_BillingViewModel != null)
                {
                    Rent.GetBilling = _BillingViewModel.GetBilling;
                }

                OnPropertyChanged(nameof(BillingViewModel));
                OnPropertyChanged(nameof(HasBilling));
            }
        }


        public bool HasBilling => _BillingViewModel != null;


        public bool HasOtherCosts
        {
            get { return Rent.HasOtherCosts; }
            set
            {
                Rent.HasOtherCosts = value;

                OnPropertyChanged(nameof(HasOtherCosts));
            }
        }


        public bool HasDataLock
        {
            get { return Rent.HasDataLock; }
            set
            {
                Rent.HasDataLock = value;

                OnPropertyChanged(nameof(HasDataLock));
            }
        }



        public DateTime StartDate
        {
            get { return Rent.StartDate; }
            set
            {
                Rent.StartDate = value; OnPropertyChanged(nameof(StartDate));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
            }
        }


        // monthly costs
        public double ColdRent
        {
            get { return Rent.ColdRent; }
            set
            {
                Rent.ColdRent = value;
                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualRent));
                OnPropertyChanged(nameof(AnnualCostsTotal));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(ColdRent)));
            }

        }


        public double FixedCostsAdvance
        {
            get { return Rent.ExtraCostsShared; }
            set
            {
                Rent.ExtraCostsShared = value;
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
            get { return Rent.ExtraCostsHeating; }
            set
            {
                Rent.ExtraCostsHeating = value;
                OnPropertyChanged(nameof(HeatingCostsAdvance));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(AnnualCostsTotal));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(HeatingCostsAdvance)));
            }
        }

        public double TotalOtherCosts => CalculateTotalOtherCosts();


        public event PropertyChangedEventHandler DataChange;


        private ObservableCollection<RoomCostsViewModel> _RoomCosts;
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


        private ObservableCollection<OtherCostItemViewModel> _OtherCosts;
        public ObservableCollection<OtherCostItemViewModel> OtherCosts
        {
            get { return _OtherCosts; }
            set
            {
                _OtherCosts = value;
                OnPropertyChanged(nameof(OtherCosts));
                OnPropertyChanged(nameof(TotalOtherCosts));
                OnPropertyChanged(nameof(AnnualOtherCosts));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(OtherCosts)));
            }
        }


        // monthly costs sums
        public double ExtraCostsTotal => FixedCostsAdvance + HeatingCostsAdvance;


        public double CostsTotal => ColdRent + ExtraCostsTotal;


        // annual interval sums
        public double AnnualRent => ColdRent * 12;


        public double AnnualExtraCosts => ExtraCostsTotal * 12;


        public double AnnualOtherCosts => TotalOtherCosts * 12;


        // annual costs sum
        public double AnnualCostsTotal => AnnualRent + AnnualExtraCosts;


        public RentViewModel(FlatViewModel flatViewModel, Rent rent)
        {
            RoomCosts = new ObservableCollection<RoomCostsViewModel>();
            OtherCosts = new ObservableCollection<OtherCostItemViewModel>();

            _flatViewModel = flatViewModel;
            Rent = rent;

            if (Rent.GetBilling != null)
            {
                BillingViewModel = new BillingViewModel(_flatViewModel, Rent.GetBilling);
            }

            if (Rent.HasOtherCosts)
            {
                OtherCosts.CollectionChanged += OtherCosts_CollectionChanged;

                GenerateOtherCosts();
            }

            OnPropertyChanged(nameof(HasBilling));


            GenerateRoomCosts();
        }


        private void OtherCosts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(OtherCosts));
            DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(OtherCosts)));
        }


        private double CalculateTotalOtherCosts()
        {
            double OtherCostsSum = 0.0;

            if (HasOtherCosts)
            {
                foreach (OtherCostItemViewModel otherCostItemViewModel in OtherCosts)
                {
                    OtherCostsSum += otherCostItemViewModel.Cost;
                }

            }

            OnPropertyChanged(nameof(AnnualOtherCosts));

            return OtherCostsSum;
        }


        public void GenerateOtherCosts()
        {
            if (Rent.OtherCosts.Count > 0)
            {
                foreach (OtherCostItem otherCostItem in Rent.OtherCosts)
                {
                    OtherCostItemViewModel otherCostItemViewModel = new OtherCostItemViewModel(otherCostItem);

                    OtherCosts.Add(otherCostItemViewModel);
                }
            }

            OnPropertyChanged(nameof(TotalOtherCosts));
            OnPropertyChanged(nameof(AnnualOtherCosts));
        }


        public void GenerateRoomCosts()
        {
            foreach (RoomCosts roomCosts in Rent.RoomCostShares)
            {
                RoomCostsViewModel roomCostsViewModel = new RoomCostsViewModel(roomCosts, this);

                RoomCosts.Add(roomCostsViewModel);
            }
        }


        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }


        public void RemoveBilling()
        {
            if (BillingViewModel != null || HasBilling)
            {
                BillingViewModel = null;
                Rent.GetBilling = null;

                OnPropertyChanged(nameof(HasBilling));
            }
        }


    }
}
// EOF