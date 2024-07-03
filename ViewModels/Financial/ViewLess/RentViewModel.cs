/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentViewModel  : BaseViewModel
 * 
 *  viewmodel for Rent model
 */
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
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


        public double AnnualRent => ColdRent.Cost * 12;
        #endregion annual interval costs


        // monthly costs
        #region monthly costs
        public CostItem ColdRent
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


        public double CostsTotal => ColdRent.Cost + ExtraCostsTotal;


        public double ExtraCostsTotal => FixedCostsAdvance.Cost + HeatingCostsAdvance.Cost;


        public CostItem FixedCostsAdvance
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


        public CostItem HeatingCostsAdvance
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


        public bool CostsHasDataLock
        {
            get { return Rent.CostsHasDataLock; }
            set
            {
                Rent.CostsHasDataLock = value;

                RentViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(CostsHasDataLock));
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

        private ObservableCollection<CostItemViewModel> _Costs;
        public ObservableCollection<CostItemViewModel> Costs
        {
            get { return _Costs; }
            set
            {
                _Costs = value;
                OnPropertyChanged(nameof(Costs));
                OnPropertyChanged(nameof(TotalOtherCosts));
                OnPropertyChanged(nameof(AnnualOtherCosts));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(Costs)));
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
            Costs = new ObservableCollection<CostItemViewModel>();

            _flatViewModel = flatViewModel;
            Rent = rent;

            if (Rent.GetBilling != null)
            {
                BillingViewModel = new BillingViewModel(_flatViewModel, Rent.GetBilling);
            }

            Costs.CollectionChanged += OtherCosts_CollectionChanged;

            GenerateCosts();

            OnPropertyChanged(nameof(HasBilling));

            GenerateRoomCosts();
        }

        #endregion constructors


        // methods
        #region methods

        public void AddCostItem(CostItemViewModel costItemViewModel)
        {
            Costs.Add(costItemViewModel);

            Rent.Costs.Add(costItemViewModel.CostItem);

            OnPropertyChanged(nameof(Costs));
        }


        private double CalculateTotalOtherCosts()
        {
            double OtherCostsSum = 0.0;

            foreach (CostItemViewModel otherCostItemViewModel in Costs)
            {
                if (!otherCostItemViewModel.CostItem.Item.Equals(ColdRent.Item)
                    && otherCostItemViewModel.CostItem.Cost != ColdRent.Cost
                    && otherCostItemViewModel.CostItem.CostShareTypes != ColdRent.CostShareTypes)
                {
                    OtherCostsSum += otherCostItemViewModel.Cost;
                }

            }

            OnPropertyChanged(nameof(AnnualOtherCosts));

            return OtherCostsSum;
        }


        public void GenerateCosts()
        {

            if (Rent.Costs.Count > 0)
            {
                foreach (CostItem costItem in Rent.Costs)
                {
                    CostItemViewModel costItemViewModel = new CostItemViewModel(costItem);

                    costItemViewModel.ValueChange += CostItemViewModel_ValueChange;

                    Costs.Add(costItemViewModel);

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

        private void CostItemViewModel_ValueChange(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(TotalOtherCosts));
        }


        private void OtherCosts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Costs));
            DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(Costs)));
        }

        #endregion event methods


    }
}
// EOF