/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  CostDisplayViewModel  : BaseViewModel
 * 
 *  viewmodel for RentCostsWindow
 *  
 *  displays a seperate window for creating additional Rent costs
 *  or for editing of existing FinancialTransactionItemViewModel instances
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Windows.Input;
using System.Windows;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class OtherCostsRentViewModel : BaseViewModel
    {
        
        // properties & fields
        #region properties

        private bool _DataLockCheckbox;
        public bool DataLockCheckbox
        {
            get { return _DataLockCheckbox; }
            set
            {
                if (RentViewModel != null)
                {
                    _DataLockCheckbox = value;
                    RentViewModel.CostsHasDataLock = value;
                }

                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(DataLock));
            }
        }


        public bool DataLock => !DataLockCheckbox;


        private readonly FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        public RentViewModel RentViewModel { get; }


        public IRoomCostsCarrier ViewModel { get; }

        #endregion properties


        // commands
        #region commands

        public ICommand AddFinacialTransactionItemCommand { get; }


        public ICommand RemoveFinancialTransactionItemCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public OtherCostsRentViewModel(RentViewModel rentViewModel)
        {

            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem(s), (s) => true);
            RemoveFinancialTransactionItemCommand = new RelayCommand((s) => RemoveFinancialTransactionItem(s), (s) => true);

            RentViewModel = rentViewModel;


            if (RentViewModel != null)
            {
                _FlatViewModel = rentViewModel.GetFlatViewModel();

                ViewModel = RentViewModel;


                if (RentViewModel.CostsHasDataLock)
                {
                    DataLockCheckbox = true;
                } 
            }

        }

        #endregion constructors


        // methods
        #region methods

        private void AddFinacialTransactionItem(object s)
        {
            FinancialTransactionItemRentViewModel otherCostItemViewModel = new FinancialTransactionItemRentViewModel(new FinancialTransactionItemRent());

            RentViewModel.AddFinacialTransactionItem(otherCostItemViewModel);

            OnPropertyChanged(nameof(RentViewModel));
        }


        private void RemoveFinancialTransactionItem(object s)
        {
            IList selection = (IList)s;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected other costs?",
                    "Remove Other TransactionSum(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<FinancialTransactionItemRentViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        RentViewModel.RemoveFinancialTransactionItemViewModel(item);

                        OnPropertyChanged(nameof(RentViewModel));
                    }
                }
            }
        }

        #endregion methods


    }
}

// EOF