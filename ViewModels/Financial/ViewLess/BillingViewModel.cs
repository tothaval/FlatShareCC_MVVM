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
        public double TotalCosts => TotalRentCosts + TotalCostsPerPeriod;


        /// <summary>
        /// returns all costs per period excluding Rent and other costs
        /// </summary>
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


        public bool CostsHasDataLock
        {
            get { return GetBilling.CostsHasDataLock; }
            set
            {
                GetBilling.CostsHasDataLock = value;

                BillingViewModelConfigurationChange?.Invoke(this, new EventArgs());

                OnPropertyChanged(nameof(CostsHasDataLock));
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

            RoomPayments = new ObservableCollection<RoomPaymentsViewModel>();

            _flatViewModel = flatViewModel;
            GetBilling = billing;

            GenerateConsumptionItemViewModels();

            GenerateFTIViewModels();

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


        public void CalculateOtherFTISum()
        {
            OtherFTISum = 0.0;

            foreach (FinancialTransactionItemViewModel item in FinancialTransactionItemViewModels)
            {
                OtherFTISum += item.Cost;
            }

            OnPropertyChanged(nameof(OtherFTISum));
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
            if (GetBilling != null && FinancialTransactionItemViewModels != null)
            {
                ConsumptionItemViewModels = new ObservableCollection<ConsumptionItemViewModel>();

                foreach (FinancialTransactionItemViewModel item in FinancialTransactionItemViewModels)
                {
                    if (item.CostShareTypes == Enums.TransactionShareTypes.Consumption)
                    {
                        GetBilling.AddConsumptionItem(item.FTI);
                    }                    
                }


                GetBilling.Check4HeatingCosts();

                foreach (ConsumptionItem item in GetBilling.ConsumptionItems)
                {
                    ConsumptionItemViewModels.Add(new ConsumptionItemViewModel(item, this));
                }
            }

            OnPropertyChanged(nameof(ConsumptionItemViewModels));
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


        internal double GetRoomConsumptionPercentage(Room room, FinancialTransactionItem transactionItem)
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



        public void RemoveCredit()
        {
            //CreditViewModel = null;
        }


        public void RemoveFinancialTransactionItemViewModel(FinancialTransactionItemViewModel costItemViewModel)
        {
            GetBilling.RemoveFinacialTransactionItem(costItemViewModel.FTI);

            GenerateFTIViewModels();

            GenerateConsumptionItemViewModels();
        }


        public void SetCredit()
        {
            //if (GetRent.BillingID != -1 && GetFlatViewModel().BillingPeriods.Count > GetRent.BillingID)
            //{
            //    BillingViewModel = GetFlatViewModel().BillingPeriods[GetRent.BillingID];
            //    HasBilling = true;
            //}
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

        private void FinancialTransactionItemViewModel_ValueChange(object? sender, EventArgs e)
        {
            GenerateConsumptionItemViewModels();

            CalculateOtherFTISum();

        }

        #endregion events


    }
}
// EOF