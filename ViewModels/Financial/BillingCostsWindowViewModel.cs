/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class BillingCostsWindowViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

        public BillingViewModel BillingViewModel { get; }


        private bool _DataLockCheckbox;
        public bool DataLockCheckbox
        {
            get { return _DataLockCheckbox; }
            set
            {
                if (BillingViewModel != null)
                {

                    _DataLockCheckbox = value;
                    BillingViewModel.CostsHasDataLock = value;
                }

                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(DataLock));
            }
        }


        public bool DataLock => !DataLockCheckbox;


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


        public ICommand CloseCommand { get; }


        public ICommand LeftPressCommand { get; }


        public ICommand RemoveFinancialTransactionItemCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public BillingCostsWindowViewModel(BillingViewModel billingViewModel)
        {

            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem(s), (s) => true);
            RemoveFinancialTransactionItemCommand = new RelayCommand((s) => RemoveFinancialTransactionItem(s), (s) => true);

            BillingViewModel = billingViewModel;
            ViewModel = BillingViewModel;

            _FlatViewModel = BillingViewModel.GetFlatViewModel();

            //if (BillingViewModel.CostsHasDataLock)
            //{
            //    DataLockCheckbox = true;
            //}

            CloseCommand = new RelayCommand((s) => Close(s), (s) => true);
            LeftPressCommand = new RelayCommand((s) => Drag(s), (s) => true);

            //CostItems.CollectionChanged += Costs_CollectionChanged;


            RegisterCostItemValueChange();

            CalculateSum();
        }
     
        #endregion constructors


        // methods
        #region methods

        private void AddFinacialTransactionItem(object s)
        {
            FinancialTransactionItemBillingViewModel otherCostItemViewModel = new FinancialTransactionItemBillingViewModel(new FinancialTransactionItemBilling());

            //otherCostItemViewModel.ValueChange += Item_ValueChange;

            BillingViewModel.AddFinacialTransactionItem(otherCostItemViewModel);

            //OnPropertyChanged(nameof(CostItems));
            OnPropertyChanged(nameof(RentViewModel));
        }


        private void CalculateSum()
        {
            //OtherFTISum = 0.0;

            //foreach (FinancialTransactionItemViewModel item in CostItems)
            //{
            //    OtherFTISum += item.TransactionSum;
            //}
        }


        private void Close(object s)
        {
            Window window = (Window)s;

            MessageBoxResult result = MessageBox.Show(window,
                $"Close Other FinancialTransactionItems window?\n\n",
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


        private void RegisterCostItemValueChange()
        {
            //foreach (FinancialTransactionItemViewModel item in CostItems)
            //{
            //    item.ValueChange += Item_ValueChange;
            //}
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

                        //OnPropertyChanged(nameof(CostItems));
                        OnPropertyChanged(nameof(RentViewModel));
                    }
                }
            }
        } 

        #endregion methods


        // events
        #region events

        private void Costs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RegisterCostItemValueChange();

            CalculateSum();
        }


        private void Item_ValueChange(object? sender, EventArgs e)
        {
            CalculateSum();
        }

        #endregion events


    }
}

// EOF