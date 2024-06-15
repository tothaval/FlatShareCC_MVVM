/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentUpdateViewModel  : BaseViewModel
 * 
 *  viewmodel for RentUpdateView
 *  
 *  allows for editing of RentViewModel
 *  
 *  is encapsulated within a RentManagementViewModel
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;


namespace SharedLivingCostCalculator.ViewModels
{
    class RentUpdateViewModel : BaseViewModel
    {

        public event EventHandler RentConfigurationChange;


        private readonly RentViewModel _rentViewModel;
        public RentViewModel RentViewModel => _rentViewModel;


        public bool SetBillingVisibility => HasBilling;
        public bool SetOtherCostsVisibility => HasOtherCosts;
        public bool SetCreditVisibility => HasCredit;

        private bool _DataLockCheckbox;
        public bool DataLockCheckbox {
            get { return _DataLockCheckbox; }
            set {
                _DataLockCheckbox = value;
                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(DataLock));
            }
        }

        public bool DataLock => !DataLockCheckbox;

        public bool HasBilling
        {
            get { return RentViewModel.HasBilling; }
            set { RentViewModel.HasBilling = value;

                if (!HasBilling)
                {
                    SelectedBillingViewModel = null;
                }

                OnPropertyChanged(nameof(HasBilling));
                OnPropertyChanged(nameof(SetBillingVisibility));

                RentConfigurationChange?.Invoke(this, EventArgs.Empty);
            }
        }

        
        public bool HasCredit
        {
            get { return RentViewModel.HasCredit; }
            set
            {
                RentViewModel.HasCredit = value;

                if (!HasCredit)
                {
                    //SelectedCreditViewModel = null;
                }

                OnPropertyChanged(nameof(HasCredit));
                OnPropertyChanged(nameof(SetCreditVisibility));

                RentConfigurationChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public bool HasOtherCosts
        {
            get { return RentViewModel.HasOtherCosts; }
            set
            {
                RentViewModel.HasOtherCosts = value;

                if (!HasOtherCosts)
                {
                    //SelectedOtherCostsViewModel = null;
                }

                OnPropertyChanged(nameof(HasOtherCosts));
                OnPropertyChanged(nameof(SetOtherCostsVisibility));

                RentConfigurationChange?.Invoke(this, EventArgs.Empty);
            }
        }



        private ValidationHelper _helper = new ValidationHelper();


        public bool HasErrors => ((INotifyDataErrorInfo)_helper).HasErrors;


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;


        public IEnumerable GetErrors(string? propertyName)
        {
            return ((INotifyDataErrorInfo)_helper).GetErrors(propertyName);
        }


        public double AnnualRent => _rentViewModel.AnnualRent;


        public double AnnualExtraCosts => _rentViewModel.AnnualExtraCosts;


        public double AnnualCostsTotal => _rentViewModel.AnnualCostsTotal;


        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;


        public double CostsTotal => ColdRent + ExtraCostsTotal;


        public DateTime? BillingStartDate => GetBillingStartDate();


        public DateTime? BillingEndDate => GetBillingEndDate();


        public double? BillingConsumedUnits => GetBillingConsumedUnits();


        public ICommand NewBillingCommand { get; }
        public ICommand NewCreditCommand { get; }
        public ICommand NewOtherCostsCommand { get; }
                

        public ICollectionView BillingViewModels { get; set; }


        private BillingViewModel _SelectedBillingViewModel;
        public BillingViewModel SelectedBillingViewModel
        {
            get { return _SelectedBillingViewModel; }
            set
            {
                _SelectedBillingViewModel = value;

                RentViewModel.BillingViewModel = SelectedBillingViewModel;

                OnPropertyChanged(nameof(SelectedBillingViewModel));
            }
        }


        private DateTime? GetBillingStartDate()
        {
            if (RentViewModel.BillingViewModel != null)
            {
                return RentViewModel.BillingViewModel.StartDate;
            }
            return null;
        }


        private DateTime? GetBillingEndDate()
        {
            if (RentViewModel.BillingViewModel != null)
            {
                return RentViewModel.BillingViewModel.EndDate;
            }
            return null;
        }


        private double? GetBillingConsumedUnits()
        {
            if (RentViewModel.BillingViewModel != null)
            {
                return RentViewModel.BillingViewModel.TotalHeatingUnitsConsumption;
            }
            return null;
        }


        public DateTime StartDate
        {
            get { return _rentViewModel.StartDate; }
            set
            {
                _rentViewModel.StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }


        public double ColdRent
        {
            get { return _rentViewModel.ColdRent; }
            set
            {
                _helper.ClearError();

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(ColdRent));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(ColdRent));
                }

                _rentViewModel.ColdRent = value;
                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(AnnualRent));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public double ExtraCostsShared
        {
            get { return _rentViewModel.FixedCostsAdvance; }
            set
            {
                _helper.ClearError();

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(ExtraCostsShared));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(ExtraCostsShared));
                }

                _rentViewModel.FixedCostsAdvance = value;
                OnPropertyChanged(nameof(ExtraCostsShared));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public double ExtraCostsHeating
        {
            get { return _rentViewModel.HeatingCostsAdvance; }
            set
            {
                _helper.ClearError();

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(ExtraCostsHeating));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(ExtraCostsHeating));
                }

                _rentViewModel.HeatingCostsAdvance = value;
                OnPropertyChanged(nameof(ExtraCostsHeating));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public RentUpdateViewModel(FlatViewModel flatViewModel, RentViewModel rentViewModel)
        {
            _rentViewModel = rentViewModel;


            if (_rentViewModel == null)
            {
                _rentViewModel = new RentViewModel(flatViewModel, new Rent());
            }

            _helper.ErrorsChanged += (_, e) => ErrorsChanged?.Invoke(this, e);

            BillingViewModels = CollectionViewSource.GetDefaultView(RentViewModel.GetFlatViewModel().BillingPeriods);
            BillingViewModels.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

            NewBillingCommand = new RelayCommand(p => AddBilling(), (s) => true);
            NewCreditCommand = new RelayCommand(p => AddCredit(), (s) => true);
            NewOtherCostsCommand = new RelayCommand(p => AddOtherCosts(), (s) => true);

            if (RentViewModel.HasBilling)
            {
                SelectedBillingViewModel = RentViewModel.BillingViewModel;
                HasBilling = true;
                OnPropertyChanged(nameof(SelectedBillingViewModel));
            }
            if (RentViewModel.HasCredit)
            {
                //SelectedCreditViewModel = RentViewModel.CreditViewModel;
                HasCredit = true;
                //OnPropertyChanged(nameof(SelectedCreditViewModel));
            }
            if (RentViewModel.HasOtherCosts)
            {
                //SelectedOtherCostsViewModel = RentViewModel.OtherCosstsViewModel;
                HasOtherCosts = true;
                //OnPropertyChanged(nameof(SelectedOtherCostsViewModel));
            }

            //DataLockCheckbox = false;
        }


        private void AddBilling()
        {
            RentViewModel.BillingViewModel = RentViewModel.GetFlatViewModel().AddBilling(RentViewModel);
            SelectedBillingViewModel = RentViewModel.BillingViewModel;
        }


        private void AddCredit()
        {
            //throw new NotImplementedException();
        }


        private void AddOtherCosts()
        {
            //throw new NotImplementedException();
        }


    }
}
// EOF