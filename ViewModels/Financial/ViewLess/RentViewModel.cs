/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentViewModel  : BaseViewModel
 * 
 *  viewmodel for Rent model
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
{
    public class RentViewModel : BaseViewModel, IRoomCostsCarrier
    {

        // Properties & Fields
        #region Properties & Fields

        // Annual Interval Costs
        #region Annual Interval Costs

        //public double AnnualCompleteCosts => AnnualCostsTotal + AnnualOtherFTISum;


        //public double AnnualCostsTotal => AnnualRent + AnnualExtraCosts;


        //public double AnnualExtraCosts => ExtraCostsTotal * 12;


        //public double AnnualOtherFTISum => OtherFTISum * 12;


        //public double AnnualRent => ColdRent * 12;


        public double FirstYearCompleteCosts => FirstYearCostsTotal + FirstYearOtherFTISum;


        public double FirstYearCostsTotal => FirstYearRent + FirstYearAdvance;


        public double FirstYearAdvance => Advance * CalculateAnnualPriceFactor();


        public double FirstYearOtherFTISum => OtherFTISum * CalculateAnnualPriceFactor();


        public double FirstYearRent => ColdRent * CalculateAnnualPriceFactor();

        #endregion annual interval costs


        // Monthly Costs
        #region Monthly Costs


        public double Advance
        {
            get { return Rent.Advance.TransactionSum; }
            set
            {
                Rent.Advance.TransactionSum = value;
                OnPropertyChanged(nameof(Advance));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(CostsAndCredits));
                OnPropertyChanged(nameof(CompleteCosts));

                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(Advance)));

                //RebuildRoomCostShares();
            }
        }


        public double ColdRent
        {
            get { return Rent.ColdRent.TransactionSum; }
            set
            {
                Rent.ColdRent.TransactionSum = value;

                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(CostsAndCredits));
                OnPropertyChanged(nameof(CompleteCosts));

                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(ColdRent)));

                //RebuildRoomCostShares();
            }

        }

        public double CompleteCosts => CostsTotal + OtherFTISum;


        public double CostsTotal => ColdRent + Advance;


        public double CostsAndCredits => ColdRent + Advance + OtherFTISum - CreditSum;


        private double _CreditSum;
        public double CreditSum
        {
            get { return _CreditSum; }
            set
            {
                _CreditSum = value;
                OnPropertyChanged(nameof(CreditSum));
                OnPropertyChanged(nameof(CostsAndCredits));
            }
        }


        private double _OtherFTISum;
        public double OtherFTISum
        {
            get { return _OtherFTISum; }
            set
            {
                _OtherFTISum = value;
                OnPropertyChanged(nameof(OtherFTISum));
                OnPropertyChanged(nameof(CostsAndCredits));

                //RebuildRoomCostShares();
            }
        }

        #endregion


        // Other Properties
        #region Other Properties

        private readonly FlatViewModel _FlatViewModel;


        public bool HasCredits
        {
            get { return Rent.HasCredits; }
            set
            {
                Rent.HasCredits = value;

                if (value == false)
                {
                    Rent.ClearCredits();
                }

                RentViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(HasCredits));
            }
        }



        private bool _HasDataLock;
        public bool NoDataLock
        {
            get { return _HasDataLock; }
            set
            {
                _HasDataLock = value;
                OnPropertyChanged(nameof(NoDataLock));
            }
        }


        public bool HasOtherCosts
        {
            get { return Rent.HasOtherCosts; }
            set
            {
                Rent.HasOtherCosts = value;

                if (value == false)
                {
                    Rent.ClearCosts();
                }

                RentViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(HasCredits));
            }
        }


        public bool IsInitialRent
        {
            get { return Rent.IsInitialRent; }
            set
            {
                Rent.IsInitialRent = value;

                OnPropertyChanged(nameof(IsInitialRent));
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


        public DateTime StartDate
        {
            get { return Rent.StartDate; }
            set
            {
                if (!Rent.IsInitialRent)
                {
                    Rent.StartDate = new Compute().DateEvaluation(value, _FlatViewModel);
                }
                else
                {
                    Rent.StartDate = value;
                }

                OnPropertyChanged(nameof(StartDate));

                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));

                ChangeDateValues();
            }
        }

        #endregion

        #endregion


        // Event Properties & Fields
        #region Event Properties & Fields

        public event PropertyChangedEventHandler DataChange;


        public event EventHandler RentViewModelConfigurationChange;

        #endregion


        // Collections
        #region Collections

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
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(FinancialTransactionItemViewModels)));
            }
        }


        public ObservableCollection<RoomCostShareRent> RoomCostShares { get; set; } = new ObservableCollection<RoomCostShareRent>();

        #endregion


        // Constructors
        #region Constructors

        public RentViewModel(FlatViewModel flatViewModel, Rent rent)
        {

            Credits = new ObservableCollection<IFinancialTransactionItem>();
            FinancialTransactionItemViewModels = new ObservableCollection<IFinancialTransactionItem>();

            _FlatViewModel = flatViewModel;
            Rent = rent;

            Credits.CollectionChanged += Credits_CollectionChanged;
            FinancialTransactionItemViewModels.CollectionChanged += OtherCosts_CollectionChanged;

            GenerateCosts();
        }

        #endregion


        // Methods
        #region Methods

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


        public double CalculateAnnualPriceFactor()
        {
            double factor = DetermineMonthsUntilYearsEnd();

            return factor;
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
            OnPropertyChanged(nameof(CostsAndCredits));
        }


        public void CalculateOtherFTISum()
        {
            OtherFTISum = 0.0;

            foreach (FinancialTransactionItemRentViewModel item in FinancialTransactionItemViewModels)
            {
                OtherFTISum += item.TransactionSum;
            }

            OnPropertyChanged(nameof(OtherFTISum));
            OnPropertyChanged(nameof(CompleteCosts));
            OnPropertyChanged(nameof(CostsAndCredits));
            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        private void ChangeDateValues()
        {
            foreach (FinancialTransactionItemRentViewModel item in Credits)
            {
                item.StartDate = StartDate;
            }

            foreach (FinancialTransactionItemRentViewModel item in FinancialTransactionItemViewModels)
            {
                item.StartDate = StartDate;
            }
        }


        private void ClearCosts()
        {
            Rent.ClearCosts();
        }


        private void ClearCredits()
        {
            Rent.ClearCredits();
        }

        public double DetermineMonthsUntilYearsEnd()
        {
            double Months = 0.0;

            DateTime start = StartDate;
            DateTime end = new DateTime(StartDate.Year, 12, 31);

            int month = 0;
            double halfmonth = 0.0;

            if (start.Day == 1 && end.Day != 14 && start.Year == end.Year)
            {
                month = end.Month - start.Month + 1;
            }

            if (start.Day == 15 && end.Day != 14 && start.Year == end.Year)
            {
                month = end.Month - start.Month;
                halfmonth += 0.5;
            }

            if (end.Day == 14 || end.Day == 15 && start.Year == end.Year)
            {
                month = end.Month - start.Month - 1;
                halfmonth = 0.5;
            }

            Months = month + halfmonth;


            return Months;
        }


        public void GenerateCosts()
        {
            if (Rent != null)
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

                //RebuildRoomCostShares();

                OnPropertyChanged(nameof(Credits));
                OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
                OnPropertyChanged(nameof(HasOtherCosts));
            }
        }


        public FlatViewModel GetFlatViewModel()
        {
            return _FlatViewModel;
        }


        public double GetFTIShareSum(TransactionShareTypesRent transactionShareTypes)
        {
            double shareSum = 0.0;

            // search consumption items
            foreach (FinancialTransactionItemRentViewModel item in FinancialTransactionItemViewModels)
            {
                // search for matching consumption item
                if (item.TransactionShareTypes == transactionShareTypes)
                {
                    shareSum += item.TransactionSum;
                }
            }

            return shareSum;
        }


        public void RecalculateRoomCosts()
        {
            RebuildRoomCostShares();
        }


        private void RebuildRoomCostShares()
        {
            RoomCostShares = new ObservableCollection<RoomCostShareRent>();

            foreach (RoomViewModel item in GetFlatViewModel().Rooms)
            {
                RoomCostShares.Add(new RoomCostShareRent(item.Room, this));
            }

            OnPropertyChanged(nameof(RoomCostShares));
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

        internal void UseRoomCosts4InitialRent(bool useRoomCosts)
        {
            if (Rent.IsInitialRent)
            {
                _FlatViewModel.UseRoomCosts4InitialRent(useRoomCosts);
            }
        }

        #endregion


        // Events
        #region

        private void CostItemViewModel_ValueChange(object? sender, EventArgs e)
        {
            CalculateOtherFTISum();

            OnPropertyChanged(nameof(CompleteCosts));
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
        }


        private void OtherCosts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenerateCosts();

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
            OnPropertyChanged(nameof(CompleteCosts));
        }

        #endregion


    }
}
// EOF