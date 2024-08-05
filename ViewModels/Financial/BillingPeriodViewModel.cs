/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingPeriodViewModel : BaseViewModel
 * 
 *  viewmodel for BillingPeriodView
 *  
 *  allows for editing of BillingViewModel
 *  
 *  is encapsulated within a BillingManagementViewModel
 *  
 *  implements INotifyDataErrorInfo
 */
using PropertyTools.Wpf;
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class BillingPeriodViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        // properties & fields
        #region properties & fields

        private readonly BillingViewModel _billingViewModel;
        public BillingViewModel BillingViewModel => _billingViewModel;


        // for access to owned windows
        private Window billingWindow;


        public ConsumptionViewModel? ConsumptionViewModel { get; set; }


        public CreditSetupViewModel CreditSetupViewModel { get; set; }


        private bool _DataLockCheckbox;
        public bool DataLockCheckbox
        {
            get { return _DataLockCheckbox; }
            set
            {
                _DataLockCheckbox = value;

                SelectedValue.HasDataLock = _DataLockCheckbox;

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
            get
            {
                if (SelectedValue != null)
                {
                    return SelectedValue.HasCredits;
                }
                return false;
            }
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
                        SelectedValue.HasCredits = value;

                        if (SelectedIndex == 2)
                        {
                            SelectedIndex = 0;
                        }
                    }
                }

                if (!HasCredit && value == true)
                {
                    SelectedValue.HasCredits = value;

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


        public bool HasOther
        {
            get
            {
                if (SelectedValue != null)
                {
                    return SelectedValue.HasOtherCosts;
                }
                return false;
            }
            set
            {

                if (value == false)
                {
                    MessageBoxResult result = MessageBox.Show(
                    $"Warning: If you uncheck this checkbox, all associated data will be lost. Proceed?",
                    "Remove Accounting Factor", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (SelectedIndex == 1)
                        {
                            SelectedIndex = 0;
                        }
                    }
                }

                if (!HasOther && value == true)
                {
                    SelectedValue.HasOtherCosts = value;

                    if (SelectedIndex != 1)
                    {
                        SelectedIndex = 1;
                    }
                }

                OnPropertyChanged(nameof(HasOther));
                OnPropertyChanged(nameof(SetOtherVisibility));
            }
        }


        public bool HasPayments
        {
            get
            {
                if (SelectedValue != null)
                {
                    return SelectedValue.HasPayments;
                }
                return false;
            }
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
                        SelectedValue.HasPayments = value;

                        if (SelectedIndex == 3)
                        {
                            SelectedIndex = 0;
                        }
                    }
                }

                if (!HasPayments && value == true)
                {
                    SelectedValue.HasPayments = value;

                    if (SelectedIndex != 3)
                    {
                        SelectedIndex = 3;
                    }
                }


                OnPropertyChanged(nameof(HasPayments));
                OnPropertyChanged(nameof(SetPaymentVisibility));
            }
        }


        private ValidationHelper _helper = new ValidationHelper();


        public BillingCostsWindowViewModel OtherCostsViewModel { get; set; }


        public PaymentManagementViewModel? PaymentManagementViewModel { get; set; }


        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;

                //OnSelectionChange();

                OnPropertyChanged(nameof(SelectedIndex));
            }
        }


        private BillingViewModel _selectedValue;
        public BillingViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                //UpdateViewModel = new RentUpdateViewModel(_flatViewModel, _selectedValue);

                PaymentManagementViewModel = new PaymentManagementViewModel(_selectedValue);
                ConsumptionViewModel = new ConsumptionViewModel(_selectedValue);
                CreditSetupViewModel = new CreditSetupViewModel(_selectedValue);
                OtherCostsViewModel = new BillingCostsWindowViewModel(_selectedValue);

                SelectedIndex = 0;

                //SelectedItemChange?.Invoke(this, new EventArgs());

                //RentUpdateSelected = true;
                //OnPropertyChanged(nameof(RentUpdateSelected));
                OnPropertyChanged(nameof(SelectedValue));

                OnPropertyChanged(nameof(PaymentManagementViewModel));
                OnPropertyChanged(nameof(ConsumptionViewModel));
                OnPropertyChanged(nameof(CreditSetupViewModel));
                OnPropertyChanged(nameof(OtherCostsViewModel));

                OnPropertyChanged(nameof(HasCredit));
                OnPropertyChanged(nameof(HasOther));
                OnPropertyChanged(nameof(HasPayments));

                OnPropertyChanged(nameof(SetCreditVisibility));
                OnPropertyChanged(nameof(SetOtherVisibility));
                OnPropertyChanged(nameof(SetPaymentVisibility));
            }
        }


        public bool SetCreditVisibility => HasCredit;

        public bool SetOtherVisibility => HasOther;

        public bool SetPaymentVisibility => HasPayments;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion event properties & fields


        // collections
        #region collections

        public ICollectionView AnnualBillings { get; }

        #endregion collections


        // commands
        #region commands

        public ICommand DeleteBillingCommand { get; }


        public ICommand NewBillingCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public BillingPeriodViewModel(FlatViewModel flatViewModel)
        {

            _FlatViewModel = flatViewModel;

            if (_FlatViewModel != null)
            {
                if (_FlatViewModel.RentUpdates.Count > 0)
                {
                    AnnualBillings = CollectionViewSource.GetDefaultView(_FlatViewModel.AnnualBillings);
                    AnnualBillings.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

                    //SelectedValue = 
                }
            }

            DeleteBillingCommand = new RelayCommand(p => DeleteAnnualBilling(p), (s) => true);
            NewBillingCommand = new RelayCommand(p => AddAnnualBilling(), (s) => true);


            _helper.ErrorsChanged += (_, e) =>
            {
                OnPropertyChanged(nameof(_helper));
                ErrorsChanged?.Invoke(this, e);
            };


            //if (BillingViewModel.HasPayments)
            //{
            //    HasPayments = true;
            //}


            //if (BillingViewModel.HasCredits)
            //{
            //    HasCredit = true;
            //}


            //if (BillingViewModel.HasDataLock)
            //{
            //    DataLockCheckbox = true;
            //}


            SelectedIndex = 0;

            //PaymentManagementViewModel = new PaymentManagementViewModel(_billingViewModel);
            //ConsumptionViewModel = new ConsumptionViewModel(_billingViewModel);
            //CreditSetupViewModel = new CreditSetupViewModel(_billingViewModel);
            //OtherCostsViewModel = new BillingCostsWindowViewModel(_billingViewModel);

            if (_FlatViewModel.AnnualBillings.Count > 0)
            {
                SelectedValue = _FlatViewModel.AnnualBillings.First();
            }
        }

        #endregion constructors


        // methods
        #region methods

        #endregion methods


        // events
        #region events

        private void AddAnnualBilling()
        {
            BillingViewModel billingViewModel = new BillingViewModel(_FlatViewModel,
                new Billing(_FlatViewModel));

            billingViewModel.Year = DateTime.Now.Year;

            _FlatViewModel.AnnualBillings.Add(billingViewModel);
            SelectedValue = billingViewModel;
            //OnPropertyChanged(nameof(HasAnnualBillings));
            OnPropertyChanged(nameof(AnnualBillings));
        }


        private void DeleteAnnualBilling(object? parameter)
        {
            IList selection = (IList)parameter;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected rent change?",
                    "Remove Rent Change(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<BillingViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        _FlatViewModel.AnnualBillings.Remove(item);

                    }

                    if (_FlatViewModel.AnnualBillings.Count > 0)
                    {
                        SelectedValue = _FlatViewModel.AnnualBillings[0];
                    }

                    //OnPropertyChanged(nameof(HasAnnualBillings));
                    OnPropertyChanged(nameof(AnnualBillings));
                }
            }
        }

        #endregion events


    }
}
// EOF