/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentViewModel  : BaseViewModel
 * 
 *  viewmodel for Rent model
 */
using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace SharedLivingCostCalculator.ViewModels
{
    public class RentViewModel : BaseViewModel, IRoomCostsCarrier
    {

        private readonly FlatViewModel _flatViewModel;


        private readonly Rent _rent;
        public Rent GetRent => _rent;


        private BillingViewModel _BillingViewModel;
        public BillingViewModel BillingViewModel
        {
            get { return _BillingViewModel; }
            set
            {
                _BillingViewModel = value;

                if (_BillingViewModel != null)
                {
                    GetRent.BillingID = _flatViewModel.BillingPeriods.IndexOf(_BillingViewModel);
                }

                OnPropertyChanged(nameof(BillingViewModel));
            }
        }


        private bool _HasBilling;
        public bool HasBilling
        {
            get { return _HasBilling; }
            set
            {
                _HasBilling = value;

                if (!HasBilling) { RemoveBilling(); }

                OnPropertyChanged(nameof(HasBilling));
            }
        }

        private bool _HasCredit;
        public bool HasCredit
        {
            get { return _HasCredit; }
            set
            {
                _HasCredit = value;

                if (!HasCredit) { RemoveCredit(); }

                OnPropertyChanged(nameof(HasCredit));
            }
        }

        private bool _HasOtherCosts;
        public bool HasOtherCosts
        {
            get { return _HasOtherCosts; }
            set
            {
                _HasOtherCosts = value;

                if (!HasOtherCosts) { RemoveOtherCosts(); }

                OnPropertyChanged(nameof(HasOtherCosts));
            }
        }

        public int ID
        {
            get { return _rent.ID; }
        }


        public DateTime StartDate
        {
            get { return _rent.StartDate; }
            set { _rent.StartDate = value; OnPropertyChanged(nameof(StartDate));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(StartDate)));
            }
        }


        // monthly costs
        public double ColdRent
        {
            get { return _rent.ColdRent; }
            set { _rent.ColdRent = value; 
                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualRent));
                OnPropertyChanged(nameof(AnnualCostsTotal));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(ColdRent)));
            }

        }


        public double FixedCostsAdvance
        {
            get { return _rent.ExtraCostsShared; }
            set { _rent.ExtraCostsShared = value;
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
            get { return _rent.ExtraCostsHeating; }
            set { _rent.ExtraCostsHeating = value; 
                OnPropertyChanged(nameof(HeatingCostsAdvance));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(CostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(AnnualCostsTotal));
                DataChange?.Invoke(this, new PropertyChangedEventArgs(nameof(HeatingCostsAdvance)));
            }
        }


        private ObservableCollection<RoomCostsViewModel> _RoomCosts;


        public event PropertyChangedEventHandler DataChange;


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


        // monthly costs sums
        public double ExtraCostsTotal => FixedCostsAdvance + HeatingCostsAdvance;


        public double CostsTotal => ColdRent + ExtraCostsTotal;


        // annual interval sums
        public double AnnualRent => ColdRent * 12;


        public double AnnualExtraCosts => ExtraCostsTotal * 12;


        // annual costs sum
        public double AnnualCostsTotal => AnnualRent + AnnualExtraCosts;


        public RentViewModel(FlatViewModel flatViewModel, Rent rent, BillingViewModel? billingViewModel = null)
        {
            _flatViewModel = flatViewModel;
            _rent = rent;         

            BillingViewModel = billingViewModel;

            SetBilling();

            OnPropertyChanged(nameof(HasBilling));
        }


        public void GenerateRoomCosts()
        {
            RoomCosts = new ObservableCollection<RoomCostsViewModel>();

            if (BillingViewModel != null)
            {
                RoomCosts = BillingViewModel.RoomCosts;
            }
            else
            {
                foreach (RoomViewModel room in _flatViewModel.Rooms)
                {
                    RoomCosts.Add(new RoomCostsViewModel(room, this));
                }
            }
        }


        public FlatViewModel GetFlatViewModel()
        {
            return _flatViewModel;
        }


        public void RemoveBilling()
        {
            BillingViewModel = null;
        }


        public void RemoveCredit()
        {
            //CreditViewModel = null;
        }


        public void RemoveOtherCosts()
        {
            //OtherCostsViewModel = null;
        }


        public void SetBilling()
        {
            if (GetRent.BillingID != -1 && GetFlatViewModel().BillingPeriods.Count > GetRent.BillingID)
            {
                BillingViewModel = GetFlatViewModel().BillingPeriods[GetRent.BillingID];
                HasBilling = true;
            }
        }


        public void SetCredit()
        {
            //if (GetRent.BillingID != -1 && GetFlatViewModel().BillingPeriods.Count > GetRent.BillingID)
            //{
            //    BillingViewModel = GetFlatViewModel().BillingPeriods[GetRent.BillingID];
            //    HasBilling = true;
            //}
        }


        public void SetOtherCosts()
        {
            //if (GetRent.BillingID != -1 && GetFlatViewModel().BillingPeriods.Count > GetRent.BillingID)
            //{
            //    BillingViewModel = GetFlatViewModel().BillingPeriods[GetRent.BillingID];
            //    HasBilling = true;
            //}
        }

    }
}
// EOF