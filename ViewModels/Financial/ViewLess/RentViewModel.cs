/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        public double AnnualCompleteCosts => AnnualCostsTotal + AnnualOtherCosts;


        public double AnnualCostsTotal => AnnualRent + AnnualExtraCosts;


        public double AnnualExtraCosts => ExtraCostsTotal * 12;


        public double AnnualOtherCosts => SumPerMonth * 12;


        public double AnnualRent => ColdRent * 12;
        #endregion annual interval costs


        // monthly costs
        #region monthly costs

        public double ColdRent
        {
            get { return Rent.ColdRent.TransactionSum; }
            set
            {
                Rent.ColdRent.TransactionSum = value;

                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(CompleteCosts));
                OnPropertyChanged(nameof(AnnualCompleteCosts));
                OnPropertyChanged(nameof(AnnualRent));
                OnPropertyChanged(nameof(AnnualCostsTotal));

                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(ColdRent)));
            }

        }


        public double CompleteCosts => CostsTotal + SumPerMonth;


        private double _CostsSumPerMonth;
        public double SumPerMonth
        {
            get { return _CostsSumPerMonth; }
            set
            {
                _CostsSumPerMonth = value;
                OnPropertyChanged(nameof(SumPerMonth));
            }
        }


        public double CostsTotal => ColdRent + ExtraCostsTotal;


        public double ExtraCostsTotal => FixedCostsAdvance + HeatingCostsAdvance;


        public double FixedCostsAdvance
        {
            get { return Rent.FixedCostsAdvance.TransactionSum; }
            set
            {
                Rent.FixedCostsAdvance.TransactionSum = value;
                OnPropertyChanged(nameof(FixedCostsAdvance));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(CompleteCosts));
                OnPropertyChanged(nameof(AnnualCompleteCosts));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(AnnualCostsTotal));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(FixedCostsAdvance)));
            }
        }


        public double HeatingCostsAdvance
        {
            get { return Rent.HeatingCostsAdvance.TransactionSum; }
            set
            {
                Rent.HeatingCostsAdvance.TransactionSum = value;
                OnPropertyChanged(nameof(HeatingCostsAdvance));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(CompleteCosts));
                OnPropertyChanged(nameof(AnnualCompleteCosts));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(AnnualCostsTotal));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(HeatingCostsAdvance)));
            }
        }

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

        private ObservableCollection<FinancialTransactionItemViewModel> _FinancialTransactionItemViewModels;
        public ObservableCollection<FinancialTransactionItemViewModel> FinancialTransactionItemViewModels
        {
            get { return _FinancialTransactionItemViewModels; }
            set
            {
                _FinancialTransactionItemViewModels = value;
                OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
                OnPropertyChanged(nameof(SumPerMonth));
                OnPropertyChanged(nameof(AnnualOtherCosts));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(FinancialTransactionItemViewModels)));
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
            FinancialTransactionItemViewModels = new ObservableCollection<FinancialTransactionItemViewModel>();

            _flatViewModel = flatViewModel;
            Rent = rent;

            if (Rent.GetBilling != null)
            {
                BillingViewModel = new BillingViewModel(_flatViewModel, Rent.GetBilling);
            }

            FinancialTransactionItemViewModels.CollectionChanged += OtherCosts_CollectionChanged;

            GenerateCosts();

            OnPropertyChanged(nameof(HasBilling));

            GenerateRoomCosts();
        }

        #endregion constructors


        // methods
        #region methods

        public void AddFinacialTransactionItem(FinancialTransactionItemViewModel costItemViewModel)
        {
            Rent.AddFinacialTransactionItem(costItemViewModel.FTI);

            GenerateCosts();

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        private void CalculateSum()
        {
            SumPerMonth = 0.0;

            foreach (FinancialTransactionItemViewModel item in FinancialTransactionItemViewModels)
            {
                SumPerMonth += item.Cost;
            }

            OnPropertyChanged(nameof(SumPerMonth));
            OnPropertyChanged(nameof(AnnualOtherCosts));
            OnPropertyChanged(nameof(CompleteCosts));
            OnPropertyChanged(nameof(AnnualCompleteCosts));
            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        public void GenerateCosts()
        {
            FinancialTransactionItemViewModels = new ObservableCollection<FinancialTransactionItemViewModel>();

            foreach (FinancialTransactionItem item in Rent.Costs)
            {
                FinancialTransactionItemViewModels.Add(new FinancialTransactionItemViewModel(item));
            }

            foreach (FinancialTransactionItemViewModel item in FinancialTransactionItemViewModels)
            {
                item.ValueChange += CostItemViewModel_ValueChange;
            }

            CalculateSum();

            OnPropertyChanged(nameof(AnnualOtherCosts));
            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
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


        public void RemoveFinancialTransactionItemViewModel(FinancialTransactionItemViewModel costItemViewModel)
        {
            Rent.RemoveCostItem(costItemViewModel.FTI);

            GenerateCosts();

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }

        #endregion methods


        // event methods
        #region event methods

        private void CostItemViewModel_ValueChange(object? sender, EventArgs e)
        {
            CalculateSum();

            OnPropertyChanged(nameof(AnnualOtherCosts));
            OnPropertyChanged(nameof(CompleteCosts));
            OnPropertyChanged(nameof(AnnualCompleteCosts));
        }


        private void OtherCosts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(FinancialTransactionItemViewModels)));

            GenerateCosts();

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
            OnPropertyChanged(nameof(CompleteCosts));
            OnPropertyChanged(nameof(AnnualCompleteCosts));
        }

        #endregion event methods


    }
}
// EOF