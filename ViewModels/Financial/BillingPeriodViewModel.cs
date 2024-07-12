/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Financial
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
                ErrorsChanged?.Invoke(this, e);
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