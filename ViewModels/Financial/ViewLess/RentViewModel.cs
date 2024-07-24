/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentViewModel  : BaseViewModel
 * 
 *  viewmodel for Rent model
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
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

        public double AnnualCompleteCosts => AnnualCostsTotal + AnnualOtherFTISum;


        public double AnnualCostsTotal => AnnualRent + AnnualExtraCosts;


        public double AnnualExtraCosts => ExtraCostsTotal * 12;


        public double AnnualOtherFTISum => OtherFTISum * 12;


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

                RebuildRoomCostShares();
            }

        }


        private double _CreditSum;
        public double CreditSum
        {
            get { return _CreditSum; }
            set
            {
                _CreditSum = value;
                OnPropertyChanged(nameof(CreditSum));
            }
        }


        public double CompleteCosts => CostsTotal + OtherFTISum;


        private double _SumPerMonth;
        public double OtherFTISum
        {
            get { return _SumPerMonth; }
            set
            {
                _SumPerMonth = value;
                OnPropertyChanged(nameof(OtherFTISum));

                RebuildRoomCostShares();
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

                RebuildRoomCostShares();
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

                RebuildRoomCostShares();
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
                    Rent.GetBilling = _BillingViewModel.Billing;
                }

                RentViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(BillingViewModel));
                OnPropertyChanged(nameof(HasBilling));

                RebuildRoomCostShares();
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


        public bool HasOtherCosts => FinancialTransactionItemViewModels.Count>0;


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

        private ObservableCollection<IFinancialTransactionItem> _Credits;
        public ObservableCollection<IFinancialTransactionItem> Credits
        {
            get { return _Credits; }
            set
            {
                _Credits = value;
                OnPropertyChanged(nameof(Credits));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(Credits)));
            }
        }


        private ObservableCollection<IFinancialTransactionItem> _FinancialTransactionItemViewModels;
        public ObservableCollection<IFinancialTransactionItem> FinancialTransactionItemViewModels
        {
            get { return _FinancialTransactionItemViewModels; }
            set
            {
                _FinancialTransactionItemViewModels = value;
                OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
                OnPropertyChanged(nameof(OtherFTISum));
                OnPropertyChanged(nameof(AnnualOtherFTISum));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(FinancialTransactionItemViewModels)));
            }
        }

        public ObservableCollection<RoomCostShareRent> RoomCostShares { get; set; }

        #endregion collections


        // constructors
        #region constructors

        public RentViewModel(FlatViewModel flatViewModel, Rent rent)
        {

            Credits = new ObservableCollection<IFinancialTransactionItem>();
            FinancialTransactionItemViewModels = new ObservableCollection<IFinancialTransactionItem>();

            _flatViewModel = flatViewModel;
            Rent = rent;

            if (Rent.GetBilling != null)
            {
                BillingViewModel = new BillingViewModel(_flatViewModel, Rent.GetBilling);
            }

            Credits.CollectionChanged += Credits_CollectionChanged;
            FinancialTransactionItemViewModels.CollectionChanged += OtherCosts_CollectionChanged;

            GenerateCosts();

            OnPropertyChanged(nameof(HasBilling));
        }

        #endregion constructors


        // methods
        #region methods

        public void AddCredit(FinancialTransactionItemRentViewModel costItemViewModel)
        {
            Rent.AddCredit(costItemViewModel.FTI);

            GenerateCosts();

            OnPropertyChanged(nameof(Credits));
        }


        public void AddFinacialTransactionItem(FinancialTransactionItemRentViewModel costItemViewModel)
        {
            Rent.AddFinacialTransactionItem(costItemViewModel.FTI);

            GenerateCosts();

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        public void CalculateCreditSum()
        {
            CreditSum = 0.0;

            foreach (FinancialTransactionItemRentViewModel item in Credits)
            {
                CreditSum += item.TransactionSum;
            }

            OnPropertyChanged(nameof(CreditSum));
            OnPropertyChanged(nameof(Credits));
        }


        public void CalculateOtherFTISum()
        {
            OtherFTISum = 0.0;

            foreach (FinancialTransactionItemRentViewModel item in FinancialTransactionItemViewModels)
            {
                OtherFTISum += item.TransactionSum;
            }

            OnPropertyChanged(nameof(OtherFTISum));
            OnPropertyChanged(nameof(AnnualOtherFTISum));
            OnPropertyChanged(nameof(CompleteCosts));
            OnPropertyChanged(nameof(AnnualCompleteCosts));
            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        public void GenerateCosts()
        {
            Credits = new ObservableCollection<IFinancialTransactionItem>();
            FinancialTransactionItemViewModels = new ObservableCollection<IFinancialTransactionItem>();

            foreach (FinancialTransactionItemRent item in Rent.Costs)
            {
                FinancialTransactionItemViewModels.Add(new FinancialTransactionItemRentViewModel(item));
            }

            foreach (FinancialTransactionItemRent item in Rent.Credits)
            {
                Credits.Add(new FinancialTransactionItemRentViewModel(item));
            }

            foreach (FinancialTransactionItemRentViewModel item in FinancialTransactionItemViewModels)
            {
                item.ValueChange += CostItemViewModel_ValueChange;
            }

            foreach (FinancialTransactionItemRentViewModel item in Credits)
            {
                item.ValueChange += CreditItemViewModel_ValueChange;
            }


            CalculateCreditSum();
            CalculateOtherFTISum();

            RebuildRoomCostShares();

            OnPropertyChanged(nameof(AnnualOtherFTISum));
            OnPropertyChanged(nameof(Credits));
            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
            OnPropertyChanged(nameof(HasOtherCosts));
        }


        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }


        public double GetFTIShareSum(TransactionShareTypesRent transactionShareTypes)
        {
            double shareSum = 0.0;

            // search consumption items
            foreach (FinancialTransactionItemRentViewModel item in FinancialTransactionItemViewModels)
            {
                // search for matching consumption item
                if (item.CostShareTypes == transactionShareTypes)
                {
                    shareSum += item.TransactionSum;
                }
            }

            return shareSum;
        }


        private void RebuildRoomCostShares()
        {
            RoomCostShares = new ObservableCollection<RoomCostShareRent>();

            foreach (RoomViewModel item in GetFlatViewModel().Rooms)
            {
                RoomCostShares.Add(new RoomCostShareRent(item.GetRoom, this));
            }

            OnPropertyChanged(nameof(RoomCostShares));
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


        public void RemoveCredit(FinancialTransactionItemRentViewModel costItemViewModel)
        {
            Rent.RemoveCredit(costItemViewModel.FTI);

            GenerateCosts();

            OnPropertyChanged(nameof(Credits));
        }


        public void RemoveFinancialTransactionItemViewModel(FinancialTransactionItemRentViewModel costItemViewModel)
        {
            Rent.RemoveFinancialTransactionItem(costItemViewModel.FTI);

            GenerateCosts();

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }

        #endregion methods


        // event methods
        #region event methods

        private void CostItemViewModel_ValueChange(object? sender, EventArgs e)
        {
            CalculateOtherFTISum();

            OnPropertyChanged(nameof(AnnualOtherFTISum));
            OnPropertyChanged(nameof(CompleteCosts));
            OnPropertyChanged(nameof(AnnualCompleteCosts));
        }

        private void CreditItemViewModel_ValueChange(object? sender, EventArgs e)
        {
            CalculateCreditSum();
        }


        private void Credits_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenerateCosts();

            OnPropertyChanged(nameof(Credits));
            OnPropertyChanged(nameof(CompleteCosts));
            OnPropertyChanged(nameof(AnnualCompleteCosts));
        }


        private void OtherCosts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenerateCosts();

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
            OnPropertyChanged(nameof(CompleteCosts));
            OnPropertyChanged(nameof(AnnualCompleteCosts));
        }

        #endregion event methods


    }
}
// EOF