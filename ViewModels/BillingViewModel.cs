/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingViewModel  : BaseViewModel
 * 
 *  viewmodel for Billing model
 *  
 *  implements IRoomCostCarrier
 */
using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace SharedLivingCostCalculator.ViewModels
{
    public class BillingViewModel : BaseViewModel, IRoomCostsCarrier
    {       

        public event PropertyChangedEventHandler DataChange;


        private readonly FlatViewModel _flatViewModel;


        public bool HasPayments
        {
            get { return GetBilling.HasPayments; }
            set
            {
                GetBilling.HasPayments = value;

                OnPropertyChanged(nameof(HasPayments));
            }
        }


        public bool HasCredit
        {
            get { return GetBilling.HasCredit; }
            set
            {
                GetBilling.HasCredit = value;

                OnPropertyChanged(nameof(HasCredit));
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


        public string Signature => $"{StartDate:d} - {EndDate:d}\n{TotalHeatingUnitsConsumption} units";


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


        public DateTime StartDate
        {
            get { return GetBilling.StartDate; }
            set {
                GetBilling.StartDate = value; OnPropertyChanged(nameof(StartDate)); 
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));


            }
        }


        public DateTime EndDate
        {
            get { return GetBilling.EndDate; }
            set { GetBilling.EndDate = value; OnPropertyChanged(nameof(EndDate));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(EndDate)));
            }
        }


        // monthly costs
        public double TotalCostsPerPeriod
        {
            get { return GetBilling.TotalCostsPerPeriod; }
            set
            {
                GetBilling.TotalCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalCostsPerPeriod));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCostsPerPeriod)));
            }
        }


        public double TotalFixedCostsPerPeriod
        {
            get { return GetBilling.TotalFixedCostsPerPeriod; }
            set
            {
                GetBilling.TotalFixedCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalFixedCostsPerPeriod));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalFixedCostsPerPeriod)));
            }
        }


        public double TotalHeatingCostsPerPeriod
        {
            get { return GetBilling.TotalHeatingCostsPerPeriod; }
            set
            {
                GetBilling.TotalHeatingCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalHeatingCostsPerPeriod));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHeatingCostsPerPeriod)));
            }
        }


        public double TotalHeatingUnitsConsumption
        {
            get { return GetBilling.TotalHeatingUnitsConsumption; }
            set
            {
                GetBilling.TotalHeatingUnitsConsumption = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsConsumption));
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
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalHeatingUnitsRoom)));
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


        public BillingViewModel(FlatViewModel flatViewModel, Billing billing)
        {
            RoomCosts = new ObservableCollection<RoomCostsViewModel>();
            RoomPayments = new ObservableCollection<RoomPaymentsViewModel>();

            _flatViewModel = flatViewModel;
            GetBilling = billing;

            GenerateRoomCosts();
            GenerateRoomPayments();
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
            if (GetBilling.RoomPayments.Count < 1)
            {
                foreach (RoomViewModel room in _flatViewModel.Rooms)
                {
                    RoomPaymentsViewModel roomPaymentsViewModel = new RoomPaymentsViewModel(new RoomPayments(room));

                    roomPaymentsViewModel.PropertyChanged += RoomPaymentsViewModel_PropertyChanged;

                    RoomPayments.Add(roomPaymentsViewModel);
                }
            }
            else
            {
                foreach (RoomPayments roomPayments in GetBilling.RoomPayments)
                {
                    RoomPaymentsViewModel roomPaymentsViewModel = new RoomPaymentsViewModel(roomPayments);

                    roomPaymentsViewModel.PropertyChanged += RoomPaymentsViewModel_PropertyChanged;

                    RoomPayments.Add(roomPaymentsViewModel);
                }
            }
        }


        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }


        private void RoomPaymentsViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

            OnPropertyChanged(nameof(RoomPayments));
            DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(RoomPayments)));
        }


        public void RemoveCredit()
        {
            //CreditViewModel = null;
        }


        public void SetCredit()
        {
            //if (GetRent.BillingID != -1 && GetFlatViewModel().BillingPeriods.Count > GetRent.BillingID)
            //{
            //    BillingViewModel = GetFlatViewModel().BillingPeriods[GetRent.BillingID];
            //    HasBilling = true;
            //}
        }


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


    }
}
// EOF