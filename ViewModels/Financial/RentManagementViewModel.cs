/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentManagementViewModel  : BaseViewModel
 * 
 *  viewmodel for RentManagementView
 *  
 *  displays the elements of ObservableCollection<RentViewModel>
 *  of the selected _FlatViewModel instance, offers management functions
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

        private AccountingViewModel _AccountingViewModel;


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


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        public bool HasRentUpdate => _FlatViewModel.RentUpdates.Count > 0;


        public ICollectionView RentUpdates { get; }


        public bool RentUpdateSelected { get; set; }


        private RentViewModel _SelectedValue;
        public RentViewModel SelectedValue
        {
            get { return _SelectedValue; }
            set
            {
                if (_SelectedValue == value) return;
                _SelectedValue = value;

                UpdateViewModel = new RentUpdateViewModel(_FlatViewModel, _SelectedValue);

                SelectedItemChange?.Invoke(this, new EventArgs());

                RentUpdateSelected = true;
                OnPropertyChanged(nameof(RentUpdateSelected));
                OnPropertyChanged(nameof(SelectedValue));
            }
        }


        private RentUpdateViewModel _UpdateViewModel;
        public RentUpdateViewModel UpdateViewModel
        {
            get { return _UpdateViewModel; }
            set
            {
                _UpdateViewModel = value;

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
            _AccountingViewModel = accountingViewModel;

            _FlatViewModel = accountingViewModel.FlatViewModel;


            AddRentUpdateCommand = new RelayCommand(p => AddRentUpdate(), (s) => true);
            AddRaiseCommand = new RelayCommand(p => AddRaise(), (s) => true);
            DeleteCommand = new RelayCommand(p => DeleteRentUpdate(p), (s) => true);

            if (_FlatViewModel != null)
            {
                if (_FlatViewModel.RentUpdates.Count > 0)
                {
                    RentUpdates = CollectionViewSource.GetDefaultView(_FlatViewModel.RentUpdates);
                    RentUpdates.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

                    SelectedValue = _FlatViewModel.GetMostRecentRent();
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
                _FlatViewModel,
                new Rent(_FlatViewModel,
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


            _FlatViewModel.RentUpdates.Add(rentViewModel);
            SelectedValue = rentViewModel;
            OnPropertyChanged(nameof(HasRentUpdate));
            OnPropertyChanged(nameof(RentUpdates));
        }


        private void AddRentUpdate()
        {
            RentViewModel rentViewModel = new RentViewModel(
                _FlatViewModel,
                new Rent(_FlatViewModel,
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    new FinancialTransactionItemRent() { TransactionItem = new LanguageResourceStrings().IDF_Rent, TransactionShareTypes = Enums.TransactionShareTypesRent.Area },
                    new FinancialTransactionItemRent() { TransactionItem = new LanguageResourceStrings().IDF_FixedCosts, TransactionShareTypes = Enums.TransactionShareTypesRent.Area },
                    new FinancialTransactionItemBilling() { TransactionItem = new LanguageResourceStrings().IDF_HeatingCosts, TransactionShareTypes = Enums.TransactionShareTypesBilling.Consumption }
                    )
                );

            _FlatViewModel.RentUpdates.Add(rentViewModel);
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
                        _FlatViewModel.RentUpdates.Remove(item);

                    }

                    if (_FlatViewModel.RentUpdates.Count > 0)
                    {
                        SelectedValue = _FlatViewModel.RentUpdates[0];
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