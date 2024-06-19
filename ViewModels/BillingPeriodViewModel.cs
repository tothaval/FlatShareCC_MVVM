/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingPeriodViewModel : BaseViewModel
 * 
 *  viewmodel for BillingPeriodView
 *  
 *  allows for editing of BillingViewModel
 *  
 *  is encapsulated within a BillingManagementViewModel
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace SharedLivingCostCalculator.ViewModels
{
    class BillingPeriodViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        private readonly BillingViewModel _billingViewModel;
        public BillingViewModel BillingViewModel => _billingViewModel;


        private ValidationHelper _helper = new ValidationHelper();


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;


        public bool HasErrors => _helper.HasErrors;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;

                OnSelectionChange();

                OnPropertyChanged(nameof(SelectedIndex));
            }
        }


        public bool SetPaymentVisibility => HasPayments;
        public bool HasPayments
        {
            get { return BillingViewModel.HasPayments; }
            set
            {
                BillingViewModel.HasPayments = value;

                if (HasPayments)
                {
                    if (SelectedIndex != 1)
                    {
                        SelectedIndex = 1;
                    }
                }
                else
                {
                    if (SelectedIndex == 1)
                    {
                        SelectedIndex = 0;
                    }
                }

                OnPropertyChanged(nameof(HasPayments));
                OnPropertyChanged(nameof(SetPaymentVisibility));
            }
        }


        public bool SetCreditVisibility => HasCredit;
        public bool HasCredit
        {
            get { return BillingViewModel.HasCredit; }
            set
            {
                BillingViewModel.HasCredit = value;

                if (HasCredit)
                {
                    if (SelectedIndex != 2)
                    {
                        SelectedIndex = 2;
                    }
                }
                else
                {
                    if (SelectedIndex == 2)
                    {
                        SelectedIndex = 0;
                    }
                }



                OnPropertyChanged(nameof(HasCredit));
                OnPropertyChanged(nameof(SetCreditVisibility));
            }
        }


        private bool _DataLockCheckbox;
        public bool DataLockCheckbox
        {
            get { return _DataLockCheckbox; }
            set
            {
                _DataLockCheckbox = value;
                _billingViewModel.HasDataLock = _DataLockCheckbox;
                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(DataLock));
            }
        }


        public bool DataLock => !DataLockCheckbox;


        public DateTime StartDate
        {
            get { return _billingViewModel.StartDate; ; }
            set
            {
                _billingViewModel.StartDate = value;
                OnPropertyChanged(nameof(StartDate));

                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));
                                    
                if (_billingViewModel.StartDate > _billingViewModel.EndDate)
                {
                    _helper.AddError("start date must be before enddate", nameof(StartDate));
                }
            }
        }


        public DateTime EndDate
        {
            get { return _billingViewModel.EndDate; ; }
            set
            {
                _billingViewModel.EndDate = value;
                OnPropertyChanged(nameof(EndDate));

                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));

                if (StartDate == EndDate || EndDate < StartDate)
                {
                    _helper.AddError("start date must be before enddate", nameof(EndDate));
                }
            }
        }


        // combined costs of fixed costs and heating costs
        // costs need to take RoomPayments per room into consideration
        public double TotalCostsPerPeriod
        {
            get { return _billingViewModel.TotalCostsPerPeriod; }
            set
            {
                _helper.ClearError(nameof(TotalCostsPerPeriod));

                if (Double.IsNaN(value))
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

                _billingViewModel.TotalCostsPerPeriod = value;

                OnPropertyChanged(nameof(TotalCostsPerPeriod));
            }
        }


        // fixed costs
        // can be calculated per room using
        // (((room area) + (shared space)/(amount of Rooms))/(total area)) * fixed costs
        public double TotalFixedCostsPerPeriod
        {
            get { return _billingViewModel.TotalFixedCostsPerPeriod; }
            set
            {
                _helper.ClearError(nameof(TotalCostsPerPeriod));
                _helper.ClearError(nameof(TotalFixedCostsPerPeriod));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalFixedCostsPerPeriod));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalFixedCostsPerPeriod));
                }
                        

                _billingViewModel.TotalFixedCostsPerPeriod = value;

                OnPropertyChanged(nameof(TotalFixedCostsPerPeriod));


                _billingViewModel.TotalHeatingCostsPerPeriod = TotalCostsPerPeriod - TotalFixedCostsPerPeriod;
                OnPropertyChanged(nameof(TotalHeatingCostsPerPeriod));
            }
        }


        // heating costs 
        // shared space heating costs can be devided by the number of Rooms
        // room based heating costs must take heating units constumption into
        // account
        public double TotalHeatingCostsPerPeriod
        {
            get { return _billingViewModel.TotalHeatingCostsPerPeriod; }
            set
            {
                _helper.ClearError(nameof(TotalCostsPerPeriod));
                _helper.ClearError(nameof(TotalHeatingCostsPerPeriod));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalHeatingCostsPerPeriod));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalHeatingCostsPerPeriod));
                }

                _billingViewModel.TotalHeatingCostsPerPeriod = value;
                OnPropertyChanged(nameof(TotalHeatingCostsPerPeriod));


                _billingViewModel.TotalFixedCostsPerPeriod = TotalCostsPerPeriod - TotalHeatingCostsPerPeriod;
                OnPropertyChanged(nameof(TotalFixedCostsPerPeriod));
            }
        }
             

        public PaymentManagementViewModel? PaymentManagementViewModel { get; set; }


        public ConsumptionViewModel? ConsumptionViewModel { get; set; }


        public ObservableCollection<RoomCostsViewModel> RoomCosts => _billingViewModel.RoomCosts;

                
        public ICommand NewCreditCommand { get; }


        public BillingPeriodViewModel(FlatViewModel flatViewModel, BillingViewModel billingViewModel)
        {
            NewCreditCommand = new RelayCommand(p => AddCredit(), (s) => true);


            _billingViewModel = billingViewModel;

            if (_billingViewModel == null)
            {
                Billing billing = new Billing(flatViewModel);


                _billingViewModel = new BillingViewModel(flatViewModel, billing);
            }


            _helper.ErrorsChanged += (_, e) =>
            {
                OnPropertyChanged(nameof(_helper));
                this.ErrorsChanged?.Invoke(this, e);
            };


            if (BillingViewModel.HasPayments)
            {
                HasPayments = true;
            }


            if (BillingViewModel.HasCredit)
            {
                HasCredit = true;
            }


            if (BillingViewModel.HasDataLock)
            {
                DataLockCheckbox = true;
            }


            SelectedIndex = 0;

            PaymentManagementViewModel = new PaymentManagementViewModel(_billingViewModel);
            ConsumptionViewModel = new ConsumptionViewModel(_billingViewModel);
        }


        private void AddCredit()
        {
            //throw new NotImplementedException();
        }


        private void OnSelectionChange()
        {
            switch (SelectedIndex)
            {
                case 0:

                default:
                    break;
            }
        }


    }
}
// EOF