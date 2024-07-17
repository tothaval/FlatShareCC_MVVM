/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  CostDisplayViewModel  : BaseViewModel
 * 
 *  viewmodel for CostsView
 *  
 *  displays a seperate window for creating
 *  or editing of FinancialTransactionItemViewModel instances
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class CostsViewModel : BaseViewModel
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


        //private double _SumPerMonth;
        //public double SumPerMonth
        //{
        //    get { return _SumPerMonth; }
        //    set
        //    {
        //        _SumPerMonth = value;
        //        OnPropertyChanged(nameof(SumPerMonth));
        //    }
        //}

        public RentViewModel RentViewModel { get; }


        public IRoomCostsCarrier ViewModel { get; }

        #endregion properties


        // commands
        #region commands

        public ICommand AddFinacialTransactionItemCommand { get; }


        public ICommand CloseCommand { get; }


        public ICommand DuplicateCostItemCommand { get; }


        public ICommand LeftPressCommand { get; }


        public ICommand RemoveFinancialTransactionItemCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public CostsViewModel(BillingViewModel billingViewModel)
        {

            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem_Billing(s), (s) => true);
            DuplicateCostItemCommand = new RelayCommand((s) => DuplicateCostItem_Billing(s), (s) => true);
            RemoveFinancialTransactionItemCommand = new RelayCommand((s) => RemoveFinancialTransactionItem_Billing(s), (s) => true);

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



        public CostsViewModel(RentViewModel rentViewModel)
        {

            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem(s), (s) => true);
            DuplicateCostItemCommand = new RelayCommand((s) => DuplicateCostItem(s), (s) => true);
            RemoveFinancialTransactionItemCommand = new RelayCommand((s) => RemoveFinancialTransactionItem(s), (s) => true);

            RentViewModel = rentViewModel;

            _FlatViewModel = rentViewModel.GetFlatViewModel();

            ViewModel = RentViewModel;

            if (RentViewModel.CostsHasDataLock)
            {
                DataLockCheckbox = true;
            }

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
            FinancialTransactionItemViewModel otherCostItemViewModel = new FinancialTransactionItemViewModel(new FinancialTransactionItem());

            //otherCostItemViewModel.ValueChange += Item_ValueChange;
            
            RentViewModel.AddFinacialTransactionItem(otherCostItemViewModel);

            //OnPropertyChanged(nameof(CostItems));
            OnPropertyChanged(nameof(RentViewModel));
        }


        private void AddFinacialTransactionItem_Billing(object s)
        {
            FinancialTransactionItemViewModel otherCostItemViewModel = new FinancialTransactionItemViewModel(new FinancialTransactionItem());

            //otherCostItemViewModel.ValueChange += Item_ValueChange;

            BillingViewModel.AddFinacialTransactionItem(otherCostItemViewModel);

            //OnPropertyChanged(nameof(CostItems));
            OnPropertyChanged(nameof(BillingViewModel));
        }


        private void DuplicateCostItem(object s)
        {
            IList selection = (IList)s;

            if (selection != null)
            {
                var selected = selection.Cast<FinancialTransactionItemViewModel>().ToArray();

                if (selected != null && selected.Length > 0)
                {
                    FinancialTransactionItemViewModel costItemViewModel = selected.First();

                    FinancialTransactionItemViewModel otherCostItemViewModel = new FinancialTransactionItemViewModel(new FinancialTransactionItem());

                    string financialTransactionItem = costItemViewModel.Item;
                    TransactionShareTypes transactionShare = costItemViewModel.CostShareTypes;
                    double transactionSum = costItemViewModel.Cost;

                    otherCostItemViewModel.FTI.TransactionItem = financialTransactionItem;
                    otherCostItemViewModel.FTI.TransactionShareTypes = transactionShare;
                    otherCostItemViewModel.FTI.TransactionSum = transactionSum;

                    otherCostItemViewModel.ValueChange += Item_ValueChange;

                    RentViewModel.AddFinacialTransactionItem(otherCostItemViewModel);

                    //OnPropertyChanged(nameof(CostItems));
                    OnPropertyChanged(nameof(RentViewModel)); 
                }
            }

                 
        }


        private void DuplicateCostItem_Billing(object s)
        {
            IList selection = (IList)s;

            if (selection != null)
            {
                var selected = selection.Cast<FinancialTransactionItemViewModel>().ToArray();

                if (selected != null && selected.Length > 0)
                {
                    FinancialTransactionItemViewModel costItemViewModel = selected.First();

                    FinancialTransactionItemViewModel otherCostItemViewModel = new FinancialTransactionItemViewModel(new FinancialTransactionItem());

                    string financialTransactionItem = costItemViewModel.Item;
                    TransactionShareTypes transactionShare = costItemViewModel.CostShareTypes;
                    double transactionSum = costItemViewModel.Cost;

                    otherCostItemViewModel.FTI.TransactionItem = financialTransactionItem;
                    otherCostItemViewModel.FTI.TransactionShareTypes = transactionShare;
                    otherCostItemViewModel.FTI.TransactionSum = transactionSum;

                    otherCostItemViewModel.ValueChange += Item_ValueChange;

                    //BillingViewModel.AddCostItem(otherCostItemViewModel);

                    //OnPropertyChanged(nameof(CostItems));
                    OnPropertyChanged(nameof(BillingViewModel)); 
                }
            }
        }


        private void CalculateSum()
        {
            //SumPerMonth = 0.0;

            //foreach (FinancialTransactionItemViewModel item in CostItems)
            //{
            //    SumPerMonth += item.Cost;
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
                    var selected = selection.Cast<FinancialTransactionItemViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        RentViewModel.RemoveFinancialTransactionItemViewModel(item);

                        //OnPropertyChanged(nameof(CostItems));
                        OnPropertyChanged(nameof(RentViewModel));
                    }
                }
            }
        }


        private void RemoveFinancialTransactionItem_Billing(object s)
        {
            IList selection = (IList)s;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected other costs?",
                    "Remove Other TransactionSum(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<FinancialTransactionItemViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        BillingViewModel.RemoveFinancialTransactionItemViewModel(item);

                        //OnPropertyChanged(nameof(CostItems));
                        OnPropertyChanged(nameof(BillingViewModel));
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