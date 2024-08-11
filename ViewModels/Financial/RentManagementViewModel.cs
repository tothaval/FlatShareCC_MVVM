/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentManagementViewModel  : BaseViewModel
 * 
 *  viewmodel for RentManagementView
 *  
 *  displays the elements of ObservableCollection<RentViewModel>
 *  of the selected FlatViewModel instance, offers management functions
 *  to add or remove elements to the collection
 *  
 *  holds an instance of RentUpdateViewModel, which is responsible
 *  for editing the data of RentViewModel instances
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
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
    public class RentManagementViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

        private AccountingViewModel _accountingViewModel;

        public bool DataLockCheckbox
        {
            get
            {
                if (UpdateViewModel != null)
                {
                    return UpdateViewModel.DataLockCheckbox;
                }

                return false;
            }

            set
            {
                UpdateViewModel.DataLockCheckbox = value;
                OnPropertyChanged(nameof(DataLockCheckbox));
            }

        }



        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;


        //public bool HasDataLock => GetSelectedValueHasDataLock();


        //private bool GetSelectedValueHasDataLock()
        //{
        //    if (SelectedValue != null)
        //    {
        //         return SelectedValue.HasDataLock; 
        //    }

        //    return false;
        //}

        public bool HasRentUpdate => _flatViewModel.RentUpdates.Count > 0;


        public ICollectionView RentUpdates { get; }


        public bool RentUpdateSelected { get; set; }


        private RentViewModel _selectedValue;
        public RentViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                UpdateViewModel = new RentUpdateViewModel(_flatViewModel, _selectedValue);

                SelectedItemChange?.Invoke(this, new EventArgs());

                RentUpdateSelected = true;
                OnPropertyChanged(nameof(RentUpdateSelected));
                OnPropertyChanged(nameof(SelectedValue));
            }
        }


        private RentUpdateViewModel _updateViewModel;
        public RentUpdateViewModel UpdateViewModel
        {
            get { return _updateViewModel; }
            set
            {
                _updateViewModel = value;

                OnPropertyChanged(nameof(UpdateViewModel));
            }
        }

        #endregion properties


        // event properties & fields
        #region event properties

        public event EventHandler SelectedItemChange;

        #endregion event properties


        // commands
        #region commands

        public ICommand AddRaiseCommand { get; }


        public ICommand AddRentUpdateCommand { get; }


        public ICommand DeleteCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public RentManagementViewModel(AccountingViewModel accountingViewModel)
        {
            _accountingViewModel = accountingViewModel;

            _flatViewModel = accountingViewModel.FlatViewModel;


            AddRentUpdateCommand = new RelayCommand(p => AddRentUpdate(), (s) => true);
            AddRaiseCommand = new RelayCommand(p => AddRaise(), (s) => true);
            DeleteCommand = new RelayCommand(p => DeleteRentUpdate(p), (s) => true);

            if (_flatViewModel != null)
            {
                if (_flatViewModel.RentUpdates.Count > 0)
                {
                    RentUpdates = CollectionViewSource.GetDefaultView(_flatViewModel.RentUpdates);
                    RentUpdates.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

                    SelectedValue = _flatViewModel.GetMostRecentRent();
                }
            }
        }

        #endregion constructors


        // methods
        #region methods

        private FinancialTransactionItemBillingViewModel CloneBillingFTI(FinancialTransactionItemBilling item)
        {
            string cause = item.TransactionItem;

            double sum = item.TransactionSum;

            TransactionShareTypesBilling shareType = item.TransactionShareTypes;

            return new FinancialTransactionItemBillingViewModel(
                new FinancialTransactionItemBilling() { TransactionItem = cause, TransactionSum = sum, TransactionShareTypes = shareType });
        }


        private FinancialTransactionItemRentViewModel CloneFTI(FinancialTransactionItemRent item)
        {
            string cause = item.TransactionItem;

            double sum = item.TransactionSum;

            TransactionShareTypesRent shareType = item.TransactionShareTypes;

            TransactionDurationTypes durationTypes = item.Duration;

            DateTime endDate = new DateTime(item.EndDate.Year, item.EndDate.Month, item.EndDate.Day);
            DateTime startDate = new DateTime(item.StartDate.Year, item.StartDate.Month, item.StartDate.Day);

            return new FinancialTransactionItemRentViewModel(
                new FinancialTransactionItemRent() {
                    TransactionItem = cause,
                    TransactionSum = sum,
                    TransactionShareTypes = shareType,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = durationTypes
                });
        }


        private void AddRaise()
        {
            RentViewModel rentViewModel = new RentViewModel(
                _flatViewModel,
                new Rent(_flatViewModel,
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    new FinancialTransactionItemRent() { TransactionItem = new LanguageResourceStrings().IDF_Rent, TransactionShareTypes = Enums.TransactionShareTypesRent.Area },
                    new FinancialTransactionItemRent() { TransactionItem = new LanguageResourceStrings().IDF_FixedCosts, TransactionShareTypes = Enums.TransactionShareTypesRent.Area },
                    new FinancialTransactionItemBilling() { TransactionItem = new LanguageResourceStrings().IDF_HeatingCosts, TransactionShareTypes = Enums.TransactionShareTypesBilling.Consumption }
                    )
                );

            rentViewModel.Rent.ColdRent = CloneFTI(SelectedValue.Rent.ColdRent).FTI;
            rentViewModel.Rent.FixedCostsAdvance = CloneFTI(SelectedValue.Rent.FixedCostsAdvance).FTI;
            rentViewModel.Rent.HeatingCostsAdvance = CloneBillingFTI(SelectedValue.Rent.HeatingCostsAdvance).FTI;


            foreach (FinancialTransactionItemRentViewModel item in SelectedValue.Credits)
            {
                rentViewModel.AddCredit(CloneFTI(item.FTI));
            }

            foreach (FinancialTransactionItemRentViewModel item in SelectedValue.FinancialTransactionItemViewModels)
            {
                rentViewModel.AddFinacialTransactionItem(CloneFTI(item.FTI));
            }

            if (rentViewModel.Credits.Count > 0)
            {
               rentViewModel.HasCredits = true;
            }


            _flatViewModel.RentUpdates.Add(rentViewModel);
            SelectedValue = rentViewModel;
            OnPropertyChanged(nameof(HasRentUpdate));
            OnPropertyChanged(nameof(RentUpdates));
        }


        private void AddRentUpdate()
        {
            RentViewModel rentViewModel = new RentViewModel(
                _flatViewModel,
                new Rent(_flatViewModel,
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    new FinancialTransactionItemRent() { TransactionItem = new LanguageResourceStrings().IDF_Rent, TransactionShareTypes = Enums.TransactionShareTypesRent.Area },
                    new FinancialTransactionItemRent() { TransactionItem = new LanguageResourceStrings().IDF_FixedCosts, TransactionShareTypes = Enums.TransactionShareTypesRent.Area },
                    new FinancialTransactionItemBilling() { TransactionItem = new LanguageResourceStrings().IDF_HeatingCosts, TransactionShareTypes = Enums.TransactionShareTypesBilling.Consumption }
                    )
                );

            _flatViewModel.RentUpdates.Add(rentViewModel);
            SelectedValue = rentViewModel;
            OnPropertyChanged(nameof(HasRentUpdate));
            OnPropertyChanged(nameof(RentUpdates));
        }


        private void DeleteRentUpdate(object? parameter)
        {
            IList selection = (IList)parameter;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected rent change?",
                    "Remove Rent Change(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<RentViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        _flatViewModel.RentUpdates.Remove(item);

                    }

                    if (_flatViewModel.RentUpdates.Count > 0)
                    {
                        SelectedValue = _flatViewModel.RentUpdates[0];
                    }

                    OnPropertyChanged(nameof(HasRentUpdate));
                    OnPropertyChanged(nameof(RentUpdates));
                }
            }
        }

        #endregion methods


    }
}
// EOF