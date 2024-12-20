/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingPeriodViewModel : BaseViewModel
 * 
 *  viewmodel for BillingPeriodView
 *  
 *  allows for editing of _BillingViewModel
 *  
 *  is encapsulated within a BillingManagementViewModel
 *  
 *  implements INotifyDataErrorInfo
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class BillingPeriodViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private BillingViewModel _BillingViewModel;
        public BillingViewModel BillingViewModel
        {
            get { return _BillingViewModel; }
            set
            {
                _BillingViewModel = value;
                OnPropertyChanged(nameof(BillingViewModel));
            }
        }


        public ConsumptionViewModel? ConsumptionViewModel { get; set; }


        public CreditSetupViewModel CreditSetupViewModel { get; set; }


        private readonly FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel
        {
            get { return _FlatViewModel; }
            set
            {
                _FlatViewModel = value;
                OnPropertyChanged(nameof(FlatViewModel));
            }
        }


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

                        if (SelectedValue != null)
                        {
                            SelectedValue.HasCredits = value;
                                                        
                            if (SelectedIndex == 3)
                            {
                                SelectedIndex = 0;
                            }
                        }
                    }
                }

                if (!HasCredit && value == true)
                {

                    if (SelectedValue != null)
                    {
                        SelectedValue.HasCredits = value; 
                    }

                    if (SelectedIndex != 3)
                    {
                        SelectedIndex = 3;
                    }
                }


                OnPropertyChanged(nameof(HasCredit));
                OnPropertyChanged(nameof(SetCreditVisibility));
            }
        }


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
                        if (SelectedValue != null)
                        {
                            SelectedValue.HasOtherCosts = value;

                            if (SelectedIndex == 2)
                            {
                                SelectedIndex = 0;
                            }
                        }
                    }
                }

                if (!HasOther && value == true)
                {
                    if (SelectedValue != null)
                    {
                        SelectedValue.HasOtherCosts = value;

                        if (SelectedIndex != 2)
                        {
                            SelectedIndex = 2;
                        }
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

                        if (SelectedIndex == 4)
                        {
                            SelectedIndex = 0;
                        }
                    }
                }

                if (!HasPayments && value == true)
                {
                    SelectedValue.HasPayments = value;

                    if (SelectedIndex != 4)
                    {
                        SelectedIndex = 4;
                    }
                }


                OnPropertyChanged(nameof(HasPayments));
                OnPropertyChanged(nameof(SetPaymentVisibility));
            }
        }


        public OtherCostsBillingViewModel OtherCostsViewModel { get; set; }


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


        private BillingViewModel _SelectedValue;
        public BillingViewModel SelectedValue
        {
            get { return _SelectedValue; }
            set
            {
                if (_SelectedValue == value) return;
                _SelectedValue = value;

                //UpdateViewModel = new RentUpdateViewModel(_flatViewModel, _selectedValue);


                if (_SelectedValue != null)
                {
                    PaymentManagementViewModel = new PaymentManagementViewModel(_SelectedValue);
                    ConsumptionViewModel = new ConsumptionViewModel(_SelectedValue);
                    CreditSetupViewModel = new CreditSetupViewModel(_SelectedValue);
                    OtherCostsViewModel = new OtherCostsBillingViewModel(_SelectedValue);

                    SelectedIndex = 0;
                }

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

        public BillingPeriodViewModel(FlatManagementViewModel flatManagementViewModel, FlatViewModel flatViewModel)
        {
            _FlatManagementViewModel = flatManagementViewModel;
            _FlatViewModel = flatViewModel;

            _FlatManagementViewModel.PropertyChanged += SelectedItem_PropertyChanged;

            DeleteBillingCommand = new RelayCommand(p => DeleteAnnualBilling(p), (s) => true);
            NewBillingCommand = new RelayCommand(p => AddAnnualBilling(), (s) => true);

            SelectFirstItem();
        }

        private void SelectedItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            FlatViewModel = _FlatManagementViewModel.SelectedItem;

            SelectFirstItem();

            OnPropertyChanged(nameof(FlatViewModel));
            OnPropertyChanged(nameof(BillingViewModel));
            OnPropertyChanged(nameof(AnnualBillings));
        }

        #endregion constructors


        // methods
        #region methods

        private void AddAnnualBilling()
        {
            BillingViewModel billingViewModel = new BillingViewModel(_FlatViewModel,
                new Billing(_FlatViewModel));

            billingViewModel.Year = DateTime.Now.Year;

            _FlatViewModel.AnnualBillings.Add(billingViewModel);
            SelectedValue = billingViewModel;

            OnPropertyChanged(nameof(AnnualBillings));
        }


        private void DeleteAnnualBilling(object? parameter)
        {
            IList selection = (IList)parameter;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected annual billing(s)?",
                    "Remove Annual Billing(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

                    OnPropertyChanged(nameof(AnnualBillings));
                }
            }
        }


        private void SelectFirstItem()
        {
            if (_FlatViewModel != null)
            {
                if (_FlatViewModel.AnnualBillings.Count > 0)
                {
                    if (_FlatViewModel.GetMostRecentBilling() != null)
                    {
                        SelectedValue = _FlatViewModel.GetMostRecentBilling();
                    }
                }
            }
        }

        #endregion methods


        // events
        #region events



        #endregion events


    }
}
// EOF