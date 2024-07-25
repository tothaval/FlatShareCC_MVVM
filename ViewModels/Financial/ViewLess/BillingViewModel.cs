/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingViewModel  : BaseViewModel
 * 
 *  viewmodel for Billing model
 *  
 *  implements IRoomCostCarrier
 */
using SharedLivingCostCalculator.Enums;
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

        // costs
        #region costs

        /// <summary>
        /// depending on configuration of billing object: Balance will return
        /// the result of all payments (and credits) per Period minus all costs per period (including Rent) or
        /// the result of all advances (and credtis) per Period minus all costs per period (exxcluding Rent)
        /// </summary>
        public double Balance => DetermineBalance();


        private double _CreditSum;
        /// <summary>
        /// sum of all other costs (FTIs) in CostView, except Heating FTI
        /// </summary>
        public double CreditSum
        {
            get { return _CreditSum; }
            set
            {
                _CreditSum = value;
                OnPropertyChanged(nameof(CreditSum));

                RebuildRoomCostShares();
            }
        }


        private double _SumPerMonth;
        /// <summary>
        /// sum of all other costs (FTIs) in CostView, except Heating FTI
        /// </summary>
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


        /// <summary>
        /// returns the sum of all advances paid per period.
        /// </summary>
        public double TotalAdvancePerPeriod => DetermineTotalAdvancePerPeriod();


        /// <summary>
        /// returns the sum of all payments paid per period and per room,
        /// payments option is for cases where more detail is either necessary or 
        /// where the tenants have some sort of conflict over money and costs
        /// and it is required to know exactly who paid how much within the periods timespan.
        /// 
        /// to reduce complexity for the users: in such cases Rent will be factored into
        /// the calculation, to prevent the user from having to split the payments into the
        /// correct payment fractions, excluding rent values.
        /// </summary>
        public double TotalPayments => CalculatePaymentsPerPeriod();


        /// <summary>
        /// returns all costs per period excluding other costs, including Rent, Fixed and Heating
        /// </summary>
        public double TotalCosts => TotalRentCosts + TotalCostsPerPeriod + OtherFTISum;


        public double TotalCostsNoPayments => TotalCostsPerPeriod + OtherFTISum;


        /// <summary>
        /// returns all costs per period excluding Rent and other costs
        /// </summary>
        public double TotalCostsPerPeriod
        {
            get { return Billing.TotalCostsPerPeriod; }

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

                Billing.TotalCostsPerPeriod = value;

                OnPropertyChanged(nameof(TotalCostsPerPeriod));

                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCostsPerPeriod)));
            }
        }


        /// <summary>
        /// returns the fixed costs per period.
        /// these are all costs besides the rent, that have no consumption value.
        /// they are calculated per room based on area share of the room. the landlord (at least in germany)
        /// splits the costs for the house and its maintenance, like insurance, water, floor lights, building
        /// superintendent and so on and distributes them to each renting party. advances are paid during the
        /// period and are calculated with the actual costs after the period is over.
        /// </summary>
        public double TotalFixedCostsPerPeriod
        {
            get { return Billing.TotalFixedCostsPerPeriod.TransactionSum; }

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


                Billing.TotalFixedCostsPerPeriod.TransactionSum = value;
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalFixedCostsPerPeriod)));



                if (TotalCostsPerPeriod - TotalHeatingCostsPerPeriod != Billing.TotalFixedCostsPerPeriod.TransactionSum)
                {
                    CalculateHeatingCosts();
                }

                OnPropertyChanged(nameof(TotalCostsPerPeriod));
                OnPropertyChanged(nameof(TotalFixedCostsPerPeriod));

                RebuildRoomCostShares();
            }
        }


        /// <summary>
        /// returns the heating costs per period.
        /// these are costs besides the rent, that have a consumption value of heating units
        /// (which might be oil, hot water, whatever). they are calculated per each room. every 
        /// rent object with no factored in billing object will base rent calculation for the user
        /// on area share.
        /// every rent object that has a billing object factored in will use the consumption
        /// values per room to determine the heating costs advance share for the room based upon
        /// the consumption habits.
        /// 
        /// advances are paid during the period and are calculated with the actual costs after
        /// the period is over.
        /// </summary>
        public double TotalHeatingCostsPerPeriod
        {
            get { return Billing.TotalHeatingCostsPerPeriod.TransactionSum; }

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

                Billing.TotalHeatingCostsPerPeriod.TransactionSum = value;
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHeatingCostsPerPeriod)));

                if (TotalCostsPerPeriod - TotalFixedCostsPerPeriod != Billing.TotalHeatingCostsPerPeriod.TransactionSum)
                {
                    CalculateFixedCosts();
                }

                OnPropertyChanged(nameof(TotalCostsPerPeriod));
                OnPropertyChanged(nameof(TotalHeatingCostsPerPeriod));

                RebuildRoomCostShares();
            }
        }


        /// <summary>
        /// !!! Currently WIP and therefor broken!!!
        /// 
        /// needed in cases where payments are factored into the billing.
        /// returns the combined rent costs for the period, based on all
        /// rent items existing within Accounting tab, that begin before
        /// period start or before period end. 
        /// 
        /// cases: 
        /// - an existing rent is used for several periods, no raises
        /// - one or several raises happen during a period, which will
        ///   lead to two or more separate rent prices for intervals
        ///   within the period
        /// </summary>
        public double TotalRentCosts => DetermineRentCostsForPaymentOption();
        
        #endregion costs


        // other properties
        #region other properties

        private Billing _Billing;
        public Billing Billing
        {
            get { return _Billing; }
            set
            {
                _Billing = value;
                OnPropertyChanged(nameof(Billing));
            }
        }


        public bool CostsHasDataLock
        {
            get { return Billing.CostsHasDataLock; }
            set
            {
                Billing.CostsHasDataLock = value;

                BillingViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(CostsHasDataLock));
            }
        }


        public DateTime EndDate
        {
            get { return Billing.EndDate; }

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
                    Billing.EndDate = value; OnPropertyChanged(nameof(EndDate));
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
            get { return Billing.HasCredits; }
            set
            {
                Billing.HasCredits = value;

                BillingViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(HasCredits));

                RebuildRoomCostShares();
            }
        }


        public bool HasDataLock
        {
            get { return Billing.HasDataLock; }
            set
            {
                Billing.HasDataLock = value;

                OnPropertyChanged(nameof(HasDataLock));
            }
        }


        public bool HasErrors => _helper.HasErrors;


        public bool HasOtherCosts => FinancialTransactionItemViewModels.Count > 0;


        public bool HasPayments
        {
            get { return Billing.HasPayments; }
            set
            {
                Billing.HasPayments = value;

                if (HasPayments)
                {
                    GenerateRoomPayments();
                }
                else
                {
                    RoomPayments.Clear();
                    Billing.RoomPayments.Clear();
                }

                BillingViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(HasPayments));
                OnPropertyChanged(nameof(Balance));

                RebuildRoomCostShares();
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


        public DateTime StartDate
        {
            get { return Billing.StartDate; }

            set
            {

                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));

                if (Billing.StartDate > Billing.EndDate)
                {
                    _helper.AddError("start date must be before enddate", nameof(StartDate));
                }
                else
                {
                    Billing.StartDate = value; OnPropertyChanged(nameof(StartDate));
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


                RebuildRoomCostShares();
            }
        }


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


        // auf änderung in den items reagieren, v.a. bei type change und wegen aktualisierung consumption
        private ObservableCollection<IFinancialTransactionItem> _FinancialTransactionItemViewModels;
        public ObservableCollection<IFinancialTransactionItem> FinancialTransactionItemViewModels
        {
            get { return _FinancialTransactionItemViewModels; }
            set
            {
                _FinancialTransactionItemViewModels = value;
                OnPropertyChanged(nameof(FinancialTransactionItemViewModels));

                //DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomConsumptionViewModels)));


                RebuildRoomCostShares();
            }
        }


        public ObservableCollection<RoomCostShareBilling> RoomCostShares { get; set; } = new ObservableCollection<RoomCostShareBilling>();


        private ObservableCollection<RoomPaymentsViewModel> _RoomPayments;
        public ObservableCollection<RoomPaymentsViewModel> RoomPayments
        {
            get { return _RoomPayments; }
            set
            {
                _RoomPayments = value;
                OnPropertyChanged(nameof(RoomPayments));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomPayments)));

                RebuildRoomCostShares();
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

            ConsumptionItemViewModels = new ObservableCollection<ConsumptionItemViewModel>();
            Credits = new ObservableCollection<IFinancialTransactionItem>();
            FinancialTransactionItemViewModels = new ObservableCollection<IFinancialTransactionItem>();

            RoomPayments = new ObservableCollection<RoomPaymentsViewModel>();

            _flatViewModel = flatViewModel;
            Billing = billing;


            GenerateFTIViewModels();

            GenerateConsumptionItemViewModels();

            GenerateCreditViewModels();

            GenerateRoomPayments();

            RebuildRoomCostShares();
        }

        #endregion constructors


        // methods
        #region methods

        public void AddCredit(FinancialTransactionItemBillingViewModel costItemViewModel)
        {
            Billing.AddCredit(costItemViewModel.FTI);

            GenerateCreditViewModels();

            OnPropertyChanged(nameof(Credits));
        }


        public void AddFinacialTransactionItem(FinancialTransactionItemBillingViewModel financialTransactionItemViewModel)
        {
            Billing.AddFinacialTransactionItem(financialTransactionItemViewModel.FTI);

            GenerateFTIViewModels();

            GenerateConsumptionItemViewModels();
        }


        public void CalculateCreditSum()
        {
            CreditSum = 0.0;

            foreach (FinancialTransactionItemBillingViewModel item in Credits)
            {
                CreditSum += item.TransactionSum;
            }

            OnPropertyChanged(nameof(CreditSum));


            RebuildRoomCostShares();
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

            if (paymentsPerPeriod > 0.0)
            {
                RebuildRoomCostShares();
            }



            return paymentsPerPeriod;
        }


        public void CalculateOtherFTISum()
        {
            OtherFTISum = 0.0;

            foreach (FinancialTransactionItemBillingViewModel item in FinancialTransactionItemViewModels)
            {
                OtherFTISum += item.TransactionSum;
            }

            OnPropertyChanged(nameof(OtherFTISum));


            RebuildRoomCostShares();
        }


        private double DetermineBalance()
        {
            if (HasPayments)
            {
                return TotalPayments - TotalCosts;
            }

            return TotalAdvancePerPeriod - TotalCostsPerPeriod;
        }

        public double DeterminePaymentMonths(ObservableCollection<RentViewModel> RentList, DateTime start, DateTime end, int i)
        {
            double Months = 0.0;

            int month = 0;
            double halfmonth = 0.0;

            if (i == 0 && RentList.Count == 1)
            {
                start = StartDate;

                end = EndDate;
            }

            else if (i == 0 && RentList.Count > 1)
            {
                start = StartDate;

                end = RentList[i + 1].StartDate - TimeSpan.FromDays(1);
            }
            else if (i < RentList.Count - 1)
            {
                start = RentList[i].StartDate;

                end = RentList[i + 1].StartDate - TimeSpan.FromDays(1);
            }
            else if (i == RentList.Count - 1)
            {
                start = RentList[i].StartDate;

                end = EndDate;
            }


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


        private double DetermineRentCostsForPaymentOption()
        {
            double rentCosts = 0.0;

            if (HasPayments)
            {
                // sort List by StartDate, ascending
                ObservableCollection<RentViewModel> RentList = FindRelevantRentViewModels();

                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;

                for (int i = 0; i < RentList.Count; i++)
                {
                    double months = DeterminePaymentMonths(RentList, start, end, i);

                    rentCosts += RentList[i].ColdRent * months;
                }

            }

            return rentCosts;
        }


        private double DetermineTotalAdvancePerPeriod()
        {
            double advance = 0.0;

            // sort List by StartDate, ascending
            ObservableCollection<RentViewModel> RentList = FindRelevantRentViewModels();

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;


            for (int i = 0; i < RentList.Count; i++)
            {
                double months = DeterminePaymentMonths(RentList, start, end, i);

                advance += RentList[i].FixedCostsAdvance * months;
                
                advance += RentList[i].HeatingCostsAdvance * months;
            }

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

                //    RentViewModel? comparer = new RentViewModel(_flatViewModel, new Rent() { StartDate = StartDate });
                //    bool firstRun = true;

                //    // building a collection of relevant rent items
                //    foreach (RentViewModel item in preSortList)
                //    {
                //        if (item.StartDate >= StartDate)
                //        {
                //            RentList.Add(item);
                //            continue;
                //        }

                //        if (item.StartDate < StartDate && firstRun)
                //        {
                //            firstRun = false;
                //            comparer = item;
                //            continue;
                //        }

                //        if (item.StartDate < StartDate && item.StartDate > comparer.StartDate)
                //        {
                //            comparer = item;
                //        }
                //    }
                //    RentList.Add(comparer);
            }

            // sort List by StartDate, ascending
            RentList = new ObservableCollection<RentViewModel>(preSortList.OrderBy(i => i.StartDate));

            return RentList;
        }


        private void GenerateConsumptionItemViewModels()
        {
            if (Billing != null && FinancialTransactionItemViewModels != null)
            {
                ConsumptionItemViewModels = new ObservableCollection<ConsumptionItemViewModel>();

                foreach (FinancialTransactionItemBillingViewModel item in FinancialTransactionItemViewModels)
                {
                    if (item.CostShareTypes == TransactionShareTypesBilling.Consumption)
                    {
                        Billing.AddConsumptionItem(item.FTI);
                    }
                }


                foreach (ConsumptionItem item in Billing.ConsumptionItems)
                {
                    ConsumptionItemViewModel consumptionItemViewModel = new ConsumptionItemViewModel(item, this);

                    consumptionItemViewModel.PropertyChanged += ConsumptionItemViewModel_PropertyChanged;

                    ConsumptionItemViewModels.Add(consumptionItemViewModel);
                }
            }

            OnPropertyChanged(nameof(ConsumptionItemViewModels));
        }


        private void GenerateCreditViewModels()
        {
            Credits = new ObservableCollection<IFinancialTransactionItem>();

            if (Billing != null)
            {
                foreach (FinancialTransactionItemBilling item in Billing.Credits)
                {
                    FinancialTransactionItemBillingViewModel FinancialTransactionItemViewModel = new FinancialTransactionItemBillingViewModel(item);

                    FinancialTransactionItemViewModel.ValueChange += CreditItem_ValueChange;

                    Credits.Add(FinancialTransactionItemViewModel);
                }
            }

            CalculateCreditSum();
        }


        private void GenerateFTIViewModels()
        {
            FinancialTransactionItemViewModels = new ObservableCollection<IFinancialTransactionItem>();

            if (Billing != null)
            {
                foreach (FinancialTransactionItemBilling item in Billing.Costs)
                {
                    FinancialTransactionItemBillingViewModel FinancialTransactionItemViewModel = new FinancialTransactionItemBillingViewModel(item);

                    FinancialTransactionItemViewModel.ValueChange += FinancialTransactionItemViewModel_ValueChange;

                    FinancialTransactionItemViewModels.Add(FinancialTransactionItemViewModel);
                }
            }

            CalculateOtherFTISum();

            OnPropertyChanged(nameof(HasOtherCosts));
        }


        public void GenerateRoomPayments()
        {
            RoomPayments.Clear();

            if (Billing.RoomPayments.Count < 1)
            {
                Billing.AddRoomPayments(FlatViewModel);
            }

            foreach (RoomPayments roomPayments in Billing.RoomPayments)
            {
                RoomPaymentsViewModel roomPaymentsViewModel = new RoomPaymentsViewModel(roomPayments);

                roomPaymentsViewModel.PropertyChanged += RoomPaymentsViewModel_PropertyChanged;

                RoomPayments.Add(roomPaymentsViewModel);
            }

            OnPropertyChanged(nameof(RoomPayments));
            OnPropertyChanged(nameof(TotalPayments));

            OnPropertyChanged(nameof(Balance));

            RebuildRoomCostShares();
        }


        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }


        public double GetFTIShareSum(TransactionShareTypesBilling transactionShareTypes)
        {
            double shareSum = 0.0;

            // search consumption items
            foreach (FinancialTransactionItemBillingViewModel item in FinancialTransactionItemViewModels)
            {
                // search for matching consumption item
                if (item.CostShareTypes == transactionShareTypes)
                {
                    shareSum += item.TransactionSum;
                }
            }

            return shareSum;
        }


        internal double GetRoomConsumptionPercentage(Room room, FinancialTransactionItemBilling transactionItem)
        {
            // search consumption items
            foreach (ConsumptionItemViewModel item in ConsumptionItemViewModels)
            {
                // search for matching consumption item
                if (item.ConsumptionCause.Equals(transactionItem.TransactionItem))
                {
                    // search room consumptions of consumption item
                    foreach (RoomConsumptionViewModel roomConsumptionItem in item.RoomConsumptionViewModels)
                    {
                        // find room matching the room argument
                        if (roomConsumptionItem.RoomConsumption.Room.RoomArea == room.RoomArea
                            && roomConsumptionItem.RoomConsumption.Room.RoomName.Equals(room.RoomName))
                        {
                            // calculate consumption share
                            // room consumption + shared consumption / room count
                            double consumptionShare =
                                roomConsumptionItem.ConsumptionValue + item.SharedConsumption / FlatViewModel.RoomCount;

                            // return consumption share / total consumed units 
                            return consumptionShare / item.ConsumedUnits;
                        }
                    }
                }
            }

            return 0.0;
        }


        private void RebuildRoomCostShares()
        {
            RoomCostShares.Clear(); // = new ObservableCollection<RoomCostShareBilling>();

            if (GetFlatViewModel() != null)
            {
                foreach (RoomViewModel item in _flatViewModel.Rooms)
                {
                    RoomCostShares.Add(new RoomCostShareBilling(item.GetRoom, this));
                }
            }

            OnPropertyChanged(nameof(RoomCostShares));
        }


        public void RemoveCredit(FinancialTransactionItemBillingViewModel costItemViewModel)
        {
            Billing.RemoveCredit(costItemViewModel.FTI);

            GenerateCreditViewModels();

            RebuildRoomCostShares();
        }


        public void RemoveFinancialTransactionItemViewModel(FinancialTransactionItemBillingViewModel costItemViewModel)
        {
            Billing.RemoveFinacialTransactionItem(costItemViewModel.FTI);

            GenerateFTIViewModels();

            GenerateConsumptionItemViewModels();

            RebuildRoomCostShares();
        }


        public void SetCredit()
        {
            //if (GetRent.BillingID != -1 && GetFlatViewModel().BillingPeriods.Count > GetRent.BillingID)
            //{
            //    BillingViewModel = GetFlatViewModel().BillingPeriods[GetRent.BillingID];
            //    HasBilling = true;
            //}
        }

        public void UpdateCosts()
        {
            RebuildRoomCostShares();
        }


        public void UpdateRoomConsumptionItemViewModels()
        {
            foreach (ConsumptionItemViewModel item in ConsumptionItemViewModels)
            {
                item.ConsumptionItem.UpdateRoomConsumptionItems(item.RoomConsumptionViewModels);

            }
        }

        #endregion methods


        // events
        #region events

        private void ConsumptionItemViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            RebuildRoomCostShares();
        }


        private void CreditItem_ValueChange(object? sender, EventArgs e)
        {
            CalculateCreditSum();
            RebuildRoomCostShares();
        }


        private void FinancialTransactionItemViewModel_ValueChange(object? sender, EventArgs e)
        {
            GenerateConsumptionItemViewModels();

            CalculateOtherFTISum();

            RebuildRoomCostShares();
        }


        private void RoomPaymentsViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RoomPayments));
            OnPropertyChanged(nameof(TotalPayments));

            OnPropertyChanged(nameof(Balance));
        }

        #endregion events


    }
}
// EOF