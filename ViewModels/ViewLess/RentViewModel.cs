/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentViewModel  : BaseViewModel
 * 
 *  viewmodel for Rent model
 */
using SharedLivingCostCalculator.Interfaces;
using SharedLivingCostCalculator.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class RentViewModel : BaseViewModel, IRoomCostsCarrier
    {

        // properties & fields
        #region properties & fields 

        // annual interval costs
        #region annual interval costs
        public double AnnualCostsTotal => AnnualRent + AnnualExtraCosts;


        public double AnnualExtraCosts => ExtraCostsTotal * 12;


        public double AnnualOtherCosts => TotalOtherCosts * 12;


        public double AnnualRent => ColdRent * 12;
        #endregion annual interval costs


        // monthly costs
        #region monthly costs
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


        public double CostsTotal => ColdRent + ExtraCostsTotal;


        public double ExtraCostsTotal => FixedCostsAdvance + HeatingCostsAdvance;


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
        #endregion monthly costs


        // other properties
        #region other properties
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

                RentViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(BillingViewModel));
                OnPropertyChanged(nameof(HasBilling));
            }
        }


        private readonly FlatViewModel _flatViewModel;


        public bool HasBilling => _BillingViewModel != null;


        public bool HasCredits
        {
            get { return Rent.HasCredits; }
            set
            {
                Rent.HasCredits = value;

                RentViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(HasCredits));
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


        public bool HasOtherCosts
        {
            get { return Rent.HasOtherCosts; }
            set
            {
                Rent.HasOtherCosts = value;

                RentViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(HasOtherCosts));
            }
        }


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


        public bool OtherCostsHasDataLock
        {
            get { return Rent.OtherCostsHasDataLock; }
            set
            {
                Rent.OtherCostsHasDataLock = value;

                RentViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(OtherCostsHasDataLock));
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
        #endregion other properties

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event PropertyChangedEventHandler DataChange;


        public event EventHandler RentViewModelConfigurationChange;

        #endregion event properties & fields


        // collections
        #region collections

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

        #endregion collections


        // constructors
        #region constructors

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

        #endregion constructors


        // methods
        #region methods

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

                    //foreach (RoomViewModel roomViewModel in GetFlatViewModel().Rooms)
                    //{
                    //    OtherCostItemViewModel otherCostItemViewModel = new OtherCostItemViewModel(otherCostItem, roomViewModel);

                    //    OtherCosts.Add(otherCostItemViewModel);
                    //}

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

        #endregion methods


        // event methods
        #region event methods
        private void OtherCosts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(OtherCosts));
            DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(OtherCosts)));
        }
        #endregion event methods


    }
}
// EOF