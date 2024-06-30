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
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    class BillingPeriodViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        // properties & fields
        #region properties & fields
        private readonly BillingViewModel _billingViewModel;
        public BillingViewModel BillingViewModel => _billingViewModel;

        public ConsumptionViewModel? ConsumptionViewModel { get; set; }


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


        private readonly FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public bool HasCredit
        {
            get { return BillingViewModel.HasCredits; }
            set
            {

                if (value == false)
                {
                    MessageBoxResult result = MessageBox.Show(
                    $"Warning: If you uncheck this checkbox, all associated data will be lost. Proceed?",
                    "Remove Accounting Factor", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //RentViewModel.RemoveBilling();
                        BillingViewModel.HasCredits = value;

                        if (SelectedIndex == 2)
                        {
                            SelectedIndex = 0;
                        }
                    }
                }

                if (!HasCredit && value == true)
                {
                    BillingViewModel.HasCredits = value;

                    if (SelectedIndex != 2)
                    {
                        SelectedIndex = 2;
                    }
                }

                OnPropertyChanged(nameof(HasCredit));
                OnPropertyChanged(nameof(SetCreditVisibility));
            }
        }


        public bool HasErrors => _helper.HasErrors;


        public bool HasPayments
        {
            get { return BillingViewModel.HasPayments; }
            set
            {
                if (value == false)
                {
                    MessageBoxResult result = MessageBox.Show(
                    $"Warning: If you uncheck this checkbox, all associated data will be lost. Proceed?",
                    "Remove Accounting Factor", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //RentViewModel.RemoveBilling();
                        BillingViewModel.HasPayments = value;

                        if (SelectedIndex == 1)
                        {
                            SelectedIndex = 0;
                        }
                    }
                }

                if (!HasPayments && value == true)
                {
                    BillingViewModel.HasPayments = value;

                    if (SelectedIndex != 1)
                    {
                        SelectedIndex = 1;
                    }
                }


                OnPropertyChanged(nameof(HasPayments));
                OnPropertyChanged(nameof(SetPaymentVisibility));
            }
        }
        private ValidationHelper _helper = new ValidationHelper();


        public PaymentManagementViewModel? PaymentManagementViewModel { get; set; }


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


        public bool SetCreditVisibility => HasCredit;
    

        public bool SetPaymentVisibility => HasPayments;


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

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion event properties & fields


        // collections
        #region collections

        public ObservableCollection<RoomCostsViewModel> RoomCosts => _billingViewModel.RoomCosts;

        #endregion collections


        // commands
        #region commands

        public ICommand CloseCommand { get; }


        public ICommand LeftPressCommand { get; }


        public ICommand NewCreditCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public BillingPeriodViewModel(FlatViewModel flatViewModel, BillingViewModel billingViewModel)
        {
            NewCreditCommand = new RelayCommand(p => AddCredit(), (s) => true);


            _billingViewModel = billingViewModel;

            if (_billingViewModel == null)
            {
                Billing billing = new Billing(flatViewModel);


                _billingViewModel = new BillingViewModel(flatViewModel, billing);
            }

            _FlatViewModel = flatViewModel;


            _helper.ErrorsChanged += (_, e) =>
            {
                OnPropertyChanged(nameof(_helper));
                this.ErrorsChanged?.Invoke(this, e);
            };


            if (BillingViewModel.HasPayments)
            {
                HasPayments = true;
            }


            if (BillingViewModel.HasCredits)
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

            CloseCommand = new RelayCommand((s) => Close(s), (s) => true);
            LeftPressCommand = new RelayCommand((s) => Drag(s), (s) => true);
        }

        #endregion constructors


        // methods
        #region methods

        private void AddCredit()
        {
            //throw new NotImplementedException();
        }


        private void Close(object s)
        {
            Window window = (Window)s;

            MessageBoxResult result = MessageBox.Show(window,
                $"Close Other Costs window?\n\n",
                "Close Window", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                window.Close();
            }
        }


        private void Drag(object s)
        {
            Window window = (Window)s;

            window.DragMove();
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

        #endregion methods


    }
}
// EOF