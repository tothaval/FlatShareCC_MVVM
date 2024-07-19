using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Enums;
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
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class CreditViewViewModel : BaseViewModel
    {

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


        private double _SumPerMonth;
        public double OtherFTISum
        {
            get { return _SumPerMonth; }
            set
            {
                _SumPerMonth = value;
                OnPropertyChanged(nameof(OtherFTISum));
            }
        }


        public RentViewModel RentViewModel { get; }


        // commands
        #region commands

        public ICommand AddFinacialTransactionItemCommand { get; }


        public ICommand CloseCommand { get; }


        public ICommand DuplicateCostItemCommand { get; }


        public ICommand LeftPressCommand { get; }


        public ICommand RemoveFinancialTransactionItemCommand { get; }

        #endregion commands


        public CreditViewViewModel(RentViewModel rentViewModel)
        {
            RentViewModel = rentViewModel;

            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem(s), (s) => true);
            DuplicateCostItemCommand = new RelayCommand((s) => DuplicateCostItem(s), (s) => true);
            RemoveFinancialTransactionItemCommand = new RelayCommand((s) => RemoveFinancialTransactionItem(s), (s) => true);

            //if (BillingViewModel.CostsHasDataLock)
            //{
            //    DataLockCheckbox = true;
            //}

            CloseCommand = new RelayCommand((s) => Close(s), (s) => true);
            LeftPressCommand = new RelayCommand((s) => Drag(s), (s) => true);
        }


        // Methods
        #region Methods
        private void AddFinacialTransactionItem(object s)
        {
            FinancialTransactionItemViewModel otherCostItemViewModel = new FinancialTransactionItemViewModel(new FinancialTransactionItem());

            //otherCostItemViewModel.ValueChange += Item_ValueChange;

            //RentViewModel.AddFinacialTransactionItem(otherCostItemViewModel);

            //OnPropertyChanged(nameof(CostItems));
            OnPropertyChanged(nameof(RentViewModel));
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

                    //RentViewModel.AddFinacialTransactionItem(otherCostItemViewModel);

                    //OnPropertyChanged(nameof(CostItems));
                    OnPropertyChanged(nameof(RentViewModel));
                }
            }


        }

        private void CalculateSum()
        {
            //OtherFTISum = 0.0;

            //foreach (FinancialTransactionItemViewModel item in CostItems)
            //{
            //    OtherFTISum += item.Cost;
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
                        //RentViewModel.RemoveFinancialTransactionItemViewModel(item);

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
