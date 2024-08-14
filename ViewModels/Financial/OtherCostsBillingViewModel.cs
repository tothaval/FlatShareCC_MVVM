/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  CostDisplayViewModel  : BaseViewModel
 * 
 *  viewmodel for BillingCostsWindow
 *  
 *  displays a seperate window for creating additional Billing costs
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
    public class OtherCostsBillingViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

        public BillingViewModel BillingViewModel { get; }


        public bool DataLock
        {
            get { return !BillingViewModel.HasDataLock; }
        }


        private readonly FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        //private double _SumPerMonth;
        //public double OtherFTISum
        //{
        //    get { return _SumPerMonth; }
        //    set
        //    {
        //        _SumPerMonth = value;
        //        OnPropertyChanged(nameof(OtherFTISum));
        //    }
        //}

        public IRoomCostsCarrier ViewModel { get; }

        #endregion properties


        // commands
        #region commands

        public ICommand AddFinacialTransactionItemCommand { get; }


        public ICommand RemoveFinancialTransactionItemCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public OtherCostsBillingViewModel(BillingViewModel billingViewModel)
        {

            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem(s), (s) => true);
            RemoveFinancialTransactionItemCommand = new RelayCommand((s) => RemoveFinancialTransactionItem(s), (s) => true);

            BillingViewModel = billingViewModel;
            ViewModel = BillingViewModel;

            _FlatViewModel = BillingViewModel.GetFlatViewModel();
            billingViewModel.PropertyChanged += BillingViewModel_PropertyChanged;

            OnPropertyChanged(nameof(DataLock));
        }

        #endregion constructors


        // methods
        #region methods

        private void AddFinacialTransactionItem(object s)
        {
            FinancialTransactionItemBillingViewModel otherCostItemViewModel = new FinancialTransactionItemBillingViewModel(new FinancialTransactionItemBilling());

            BillingViewModel.AddFinacialTransactionItem(otherCostItemViewModel);

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
                    var selected = selection.Cast<FinancialTransactionItemBillingViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        BillingViewModel.RemoveFinancialTransactionItemViewModel(item);

                        OnPropertyChanged(nameof(RentViewModel));
                    }
                }
            }
        }

        #endregion methods


        // events
        #region events

        private void BillingViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DataLock));
        }

        #endregion events


    }
}

// EOF