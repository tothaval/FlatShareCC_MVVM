/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingViewModel  : BaseViewModel
 * 
 *  viewmodel for Billing model
 *  
 *  implements IRoomCostCarrier
 */
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
{
    public class BillingViewModel : BaseViewModel, IRoomCostsCarrier, INotifyDataErrorInfo
    {

        // properties & fields
        #region properties

        // Heating
        #region Heating

        public double SharedHeatingUnitsConsumption => TotalHeatingUnitsConsumption - TotalHeatingUnitsRoom;


        public double SharedHeatingUnitsConsumptionPercentage => SharedHeatingUnitsConsumption / TotalHeatingUnitsConsumption * 100;


        public double TotalHeatingUnitsConsumption
        {
            get { return GetBilling.TotalHeatingUnitsConsumption; }
            set
            {
                GetBilling.TotalHeatingUnitsConsumption = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsConsumption));
                OnPropertyChanged(nameof(SharedHeatingUnitsConsumption));
                OnPropertyChanged(nameof(SharedHeatingUnitsConsumptionPercentage));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHeatingUnitsConsumption)));
            }
        }


        public double TotalHeatingUnitsRoom
        {
            get { return GetBilling.TotalHeatingUnitsRoom; }
            set
            {
                GetBilling.TotalHeatingUnitsRoom = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsRoom));
                OnPropertyChanged(nameof(SharedHeatingUnitsConsumption));
                OnPropertyChanged(nameof(SharedHeatingUnitsConsumptionPercentage));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHeatingUnitsRoom)));
            }
        }
        #endregion Heating


        // costs
        #region costs
        public double TotalRentCosts
        {
            get { return -1.0; }
            //get {
            //    if (BillingViewModel.RentViewModel == null)
            //    {
            //        return -1.0;
            //    }

            //    
            // DetermineAnnualRent via calculation and date checks of rent updates in flatviewmodel
            //
            //
            //    return BillingViewModel.RentViewModel.AnnualRent;

            //}
        }

        public double TotalAdvancePerPeriod => DetermineTotalAdvancePerPeriod();

        public double Balance => DetermineBalance();


        public double TotalPayments => CalculatePaymentsPerPeriod();

        public double TotalCosts => TotalRentCosts + TotalCostsPerPeriod;

        public double TotalCostsPerPeriod
        {
            get { return GetBilling.TotalCostsPerPeriod; }

            set
            {
                _helper.ClearError(nameof(TotalCostsPerPeriod));

                if (double.IsNaN(value))
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

                GetBilling.TotalCostsPerPeriod = value;

                OnPropertyChanged(nameof(TotalCostsPerPeriod));

                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCostsPerPeriod)));

            }
        }


        public double TotalFixedCostsPerPeriod
        {
            get { return GetBilling.TotalFixedCostsPerPeriod.TransactionSum; }

            set
            {
                _helper.ClearError(nameof(TotalCostsPerPeriod));
                _helper.ClearError(nameof(TotalFixedCostsPerPeriod));

                if (double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalFixedCostsPerPeriod));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalFixedCostsPerPeriod));
                }


                GetBilling.TotalFixedCostsPerPeriod.TransactionSum = value;
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalFixedCostsPerPeriod)));



                if (TotalCostsPerPeriod - TotalHeatingCostsPerPeriod != GetBilling.TotalFixedCostsPerPeriod.TransactionSum)
                {
                    CalculateHeatingCosts();
                }

                OnPropertyChanged(nameof(TotalCostsPerPeriod));
                OnPropertyChanged(nameof(TotalFixedCostsPerPeriod));
            }
        }


        public double TotalHeatingCostsPerPeriod
        {
            get { return GetBilling.TotalHeatingCostsPerPeriod.TransactionSum; }

            set
            {
                _helper.ClearError(nameof(TotalCostsPerPeriod));
                _helper.ClearError(nameof(TotalHeatingCostsPerPeriod));

                if (double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalHeatingCostsPerPeriod));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalHeatingCostsPerPeriod));
                }

                GetBilling.TotalHeatingCostsPerPeriod.TransactionSum = value;
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHeatingCostsPerPeriod)));

                if (TotalCostsPerPeriod - TotalFixedCostsPerPeriod != GetBilling.TotalHeatingCostsPerPeriod.TransactionSum)
                {
                    CalculateFixedCosts();
                }

                OnPropertyChanged(nameof(TotalCostsPerPeriod));
                OnPropertyChanged(nameof(TotalHeatingCostsPerPeriod));
            }
        }
        #endregion costs


        // other properties
        #region other properties
        private Billing _Billing;
        public Billing GetBilling
        {
            get { return _Billing; }
            set
            {
                _Billing = value;
                OnPropertyChanged(nameof(GetBilling));
            }
        }


        public DateTime EndDate
        {
            get { return GetBilling.EndDate; }

            set
            {
                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));

                if (StartDate == EndDate || EndDate < StartDate)
                {
                    _helper.AddError("start date must be before enddate", nameof(EndDate));
                }
                else
                {
                    GetBilling.EndDate = value; OnPropertyChanged(nameof(EndDate));
                    DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));

                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }


        private readonly FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public bool HasCredits
        {
            get { return GetBilling.HasCredits; }
            set
            {
                GetBilling.HasCredits = value;

                BillingViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(HasCredits));
            }
        }


        public bool HasDataLock
        {
            get { return GetBilling.HasDataLock; }
            set
            {
                GetBilling.HasDataLock = value;

                OnPropertyChanged(nameof(HasDataLock));
            }
        }


        public bool HasErrors => _helper.HasErrors;


        public bool HasPayments
        {
            get { return GetBilling.HasPayments; }
            set
            {
                GetBilling.HasPayments = value;

                if (HasPayments)
                {
                    GenerateRoomPayments();
                }

                BillingViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(HasPayments));
            }
        }


        private ValidationHelper _helper = new ValidationHelper();


        private ConsumptionItemViewModel _SelectedConsumptionItem;
        public ConsumptionItemViewModel SelectedConsumptionItem
        {
            get { return _SelectedConsumptionItem; }
            set
            {
                _SelectedConsumptionItem = value;
                OnPropertyChanged(nameof(SelectedConsumptionItem));
            }
        }


        public string Signature => $"{StartDate:d} - {EndDate:d}\n{TotalHeatingUnitsConsumption} units";


        public DateTime StartDate
        {
            get { return GetBilling.StartDate; }

            set
            {

                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));

                if (GetBilling.StartDate > GetBilling.EndDate)
                {
                    _helper.AddError("start date must be before enddate", nameof(StartDate));
                }
                else
                {
                    GetBilling.StartDate = value; OnPropertyChanged(nameof(StartDate));
                    DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));

                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        #endregion other properties

        #endregion properties


        // event properties & fields
        #region event handlers

        public event EventHandler BillingViewModelConfigurationChange;


        public event PropertyChangedEventHandler DataChange;


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        #endregion event handlers


        // collections
        #region collections

        private ObservableCollection<ConsumptionItemViewModel> _ConsumptionItemViewModels;
        public ObservableCollection<ConsumptionItemViewModel> ConsumptionItemViewModels
        {
            get { return _ConsumptionItemViewModels; }
            set
            {
                _ConsumptionItemViewModels = value;
                OnPropertyChanged(nameof(ConsumptionItemViewModels));

                //DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomConsumptionViewModels)));
            }
        }


        // auf änderung in den items reagieren, v.a. bei type change und wegen aktualisierung consumption
        private ObservableCollection<FinancialTransactionItemViewModel> _FinancialTransactionItemViewModels;
        public ObservableCollection<FinancialTransactionItemViewModel> FinancialTransactionItemViewModels
        {
            get { return _FinancialTransactionItemViewModels; }
            set
            {
                _FinancialTransactionItemViewModels = value;
                OnPropertyChanged(nameof(FinancialTransactionItemViewModels));

                //DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomConsumptionViewModels)));
            }
        }


        private ObservableCollection<RoomConsumptionViewModel> _RoomConsumptionViewModels;
        public ObservableCollection<RoomConsumptionViewModel> RoomConsumptionViewModels
        {
            get { return _RoomConsumptionViewModels; }
            set
            {
                _RoomConsumptionViewModels = value;
                OnPropertyChanged(nameof(RoomConsumptionViewModels));


                //DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomConsumptionViewModels)));
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


        private ObservableCollection<RoomPaymentsViewModel> _RoomPayments;
        public ObservableCollection<RoomPaymentsViewModel> RoomPayments
        {
            get { return _RoomPayments; }
            set
            {
                _RoomPayments = value;
                OnPropertyChanged(nameof(RoomPayments));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomPayments)));
            }
        }
        #endregion collections


        // constructors
        #region constructors

        public BillingViewModel(FlatViewModel flatViewModel, Billing billing)
        {
            _helper.ErrorsChanged += (_, e) =>
            {
                OnPropertyChanged(nameof(_helper));
                ErrorsChanged?.Invoke(this, e);
            };

            FinancialTransactionItemViewModels = new ObservableCollection<FinancialTransactionItemViewModel>();
            RoomConsumptionViewModels = new ObservableCollection<RoomConsumptionViewModel>();
            RoomCosts = new ObservableCollection<RoomCostsViewModel>();
            RoomPayments = new ObservableCollection<RoomPaymentsViewModel>();

            _flatViewModel = flatViewModel;
            GetBilling = billing;

            GenerateConsumptionItemViewModels();
            GenerateFTIViewModels();
            GenerateRoomConsumptionItems();
            GenerateRoomCosts();
            GenerateRoomPayments();
        }

        #endregion constructors


        // methods
        #region methods

        public void AddFinacialTransactionItem(FinancialTransactionItemViewModel financialTransactionItemViewModel)
        {
            GetBilling.AddFinacialTransactionItem(financialTransactionItemViewModel.FTI);

            GenerateFTIViewModels();

            GenerateConsumptionItemViewModels();

            //GenerateConsumptionItemViewModels(); // vermutlich woanders hin, zu consumption relevantem
        }

        private void CalculateFixedCosts()
        {
            TotalFixedCostsPerPeriod = TotalCostsPerPeriod - TotalHeatingCostsPerPeriod;
        }

        private void CalculateHeatingCosts()
        {
            TotalHeatingCostsPerPeriod = TotalCostsPerPeriod - TotalFixedCostsPerPeriod;
        }

        public double CalculatePaymentsPerPeriod()
        {
            double paymentsPerPeriod = 0.0;

            foreach (RoomPaymentsViewModel roomPaymentsViewModel in RoomPayments)
            {
                foreach (Payment payment in roomPaymentsViewModel.RoomPayments.Payments)
                {
                    if (payment.StartDate >= StartDate
                        && payment.StartDate <= EndDate
                        && payment.EndDate >= StartDate
                        && payment.EndDate <= EndDate
                        )
                    {
                        paymentsPerPeriod += payment.PaymentTotal;
                    }
                }


            }

            return paymentsPerPeriod;
        }

        private double DetermineBalance()
        {
            double balance = 0.0;

            if (HasPayments)
            {
                return TotalPayments - TotalCosts;
            }

            return TotalAdvancePerPeriod - TotalCostsPerPeriod;
        }


        private double DetermineTotalAdvancePerPeriod()
        {
            double advance = 0.0;

            // use these items:
            // BillingViewModel.FindRelevantRentViewModels()
            // CalculateRentCosts() (RoomCostsViewModel <= change ColdRent to AdvanceType

            //advance += months * rentViewModel.ExtraCostsTotal;

            ObservableCollection<RentViewModel> rentViewModels = new ObservableCollection<RentViewModel>();


            return advance;
        }

        public ObservableCollection<RentViewModel> FindRelevantRentViewModels()
        {
            ObservableCollection<RentViewModel> preSortList = new ObservableCollection<RentViewModel>();
            ObservableCollection<RentViewModel> RentList = new ObservableCollection<RentViewModel>();

            if (GetFlatViewModel().RentUpdates.Count > 0)
            {
                // filling the collection with potential matches
                foreach (RentViewModel rent in GetFlatViewModel().RentUpdates)
                {
                    // rent begins after Billing period ends
                    if (rent.StartDate > EndDate)
                    {
                        continue;
                    }

                    // rent begins before Billing period starts
                    if (rent.StartDate < StartDate)
                    {
                        preSortList.Add(new RentViewModel(GetFlatViewModel(), rent.Rent));
                        continue;
                    }

                    // rent begins before Billing period end
                    if (rent.StartDate < EndDate)
                    {
                        preSortList.Add(new RentViewModel(GetFlatViewModel(), rent.Rent));

                        continue;
                    }

                    // rent begins after Billing period start but before Billing period end
                    if (rent.StartDate > StartDate || rent.StartDate < EndDate)
                    {
                        preSortList.Add(new RentViewModel(GetFlatViewModel(), rent.Rent));
                    }
                }

                RentViewModel? comparer = new RentViewModel(_flatViewModel, new Rent() { StartDate = StartDate });
                bool firstRun = true;

                // building a collection of relevant rent items
                foreach (RentViewModel item in preSortList)
                {
                    if (item.StartDate >= StartDate)
                    {
                        RentList.Add(item);
                        continue;
                    }

                    if (item.StartDate < StartDate && firstRun)
                    {
                        firstRun = false;
                        comparer = item;
                        continue;
                    }

                    if (item.StartDate < StartDate && item.StartDate > comparer.StartDate)
                    {
                        comparer = item;
                    }
                }
                RentList.Add(comparer);
            }

            // sort List by StartDate, ascending
            RentList = new ObservableCollection<RentViewModel>(RentList.OrderBy(i => i.StartDate));

            return RentList;
        }


        private void GenerateConsumptionItemViewModels()
        {
            ConsumptionItemViewModels = new ObservableCollection<ConsumptionItemViewModel>();

            if (GetBilling != null && FinancialTransactionItemViewModels != null)
            {               

                foreach (FinancialTransactionItemViewModel item in FinancialTransactionItemViewModels)
                {
                    if (item.CostShareTypes == Enums.TransactionShareTypes.Consumption)
                    {
                        GetBilling.AddConsumptionItem(new ConsumptionItem(item.FTI, 0.0));
                    }
                }

                foreach (ConsumptionItem item in GetBilling.ConsumptionItems)
                {
                    ConsumptionItemViewModels.Add(new ConsumptionItemViewModel(item, this));
                }
            }
        }



        private void GenerateFTIViewModels()
        {
            FinancialTransactionItemViewModels = new ObservableCollection<FinancialTransactionItemViewModel>();

            if (GetBilling != null)
            {
                foreach (FinancialTransactionItem item in GetBilling.Costs)
                {
                    FinancialTransactionItemViewModel FinancialTransactionItemViewModel = new FinancialTransactionItemViewModel(item);

                    FinancialTransactionItemViewModel.ValueChange += FinancialTransactionItemViewModel_ValueChange;

                    FinancialTransactionItemViewModels.Add(FinancialTransactionItemViewModel);
                }
            }
        }

        private void FinancialTransactionItemViewModel_ValueChange(object? sender, EventArgs e)
        {
            GenerateConsumptionItemViewModels();
        }

        public void GenerateRoomConsumptionItems()
        {
            foreach (RoomViewModel roomViewModel in _flatViewModel.Rooms)
            {
                RoomConsumption roomConsumption = new RoomConsumption();
                roomConsumption.Room = roomViewModel.GetRoom;

                RoomConsumptionViewModels.Add(new RoomConsumptionViewModel(roomConsumption));
            }
        }


        public void GenerateRoomCosts()
        {
            foreach (RoomCosts roomCosts in GetBilling.RoomCostsConsumptionValues)
            {
                RoomCostsViewModel roomCostsViewModel = new RoomCostsViewModel(roomCosts, this);
                roomCostsViewModel.HeatingUnitsChange += RoomCostsViewModel_HeatingUnitsChange;

                RoomCosts.Add(roomCostsViewModel);
            }
        }


        public void GenerateRoomPayments()
        {
            RoomPayments = new ObservableCollection<RoomPaymentsViewModel>();

            if (GetBilling.RoomPayments.Count < 1)
            {
                foreach (RoomViewModel room in _flatViewModel.Rooms)
                {
                    RoomPaymentsViewModel roomPaymentsViewModel = new RoomPaymentsViewModel(new RoomPayments(room));

                    RoomPayments.Add(roomPaymentsViewModel);
                }
            }
            else
            {
                foreach (RoomPayments roomPayments in GetBilling.RoomPayments)
                {
                    RoomPaymentsViewModel roomPaymentsViewModel = new RoomPaymentsViewModel(roomPayments);

                    RoomPayments.Add(roomPaymentsViewModel);
                }
            }
        }


        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }


        public void RemoveCredit()
        {
            //CreditViewModel = null;
        }


        public void RemoveFinancialTransactionItemViewModel(FinancialTransactionItemViewModel costItemViewModel)
        {
            GetBilling.RemoveFinacialTransactionItem(costItemViewModel.FTI);

            GenerateFTIViewModels();

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        public void SetCredit()
        {
            //if (GetRent.BillingID != -1 && GetFlatViewModel().BillingPeriods.Count > GetRent.BillingID)
            //{
            //    BillingViewModel = GetFlatViewModel().BillingPeriods[GetRent.BillingID];
            //    HasBilling = true;
            //}
        }

        #endregion methods


        // events
        #region events

        private void RoomCosts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RoomCosts));
            DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomCosts)));
        }


        private void RoomCostsViewModel_HeatingUnitsChange(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(RoomCosts));
            DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomCosts)));
        }

        #endregion events


    }
}
// EOF