/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  CostDisplayViewModel  : BaseViewModel
 * 
 *  viewmodel for CostsView
 *  
 *  displays a seperate window for creating
 *  or editing of CostItemViewModel instances
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class CostsViewModel : BaseViewModel
    {

        // properties & fields
        #region properties

        private bool _DataLockCheckbox;
        public bool DataLockCheckbox
        {
            get { return _DataLockCheckbox; }
            set
            {
                _DataLockCheckbox = value;
                _RentViewModel.CostsHasDataLock = value;
                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(DataLock));
            }
        }


        public bool DataLock => !DataLockCheckbox;


        private readonly FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        private double _CostsSumPerMonth;
        public double CostsSumPerMonth
        {
            get { return _CostsSumPerMonth; }
            set
            {
                _CostsSumPerMonth = value;
                OnPropertyChanged(nameof(CostsSumPerMonth));
            }
        }


        private readonly RentViewModel _RentViewModel;

        #endregion properties


        // collections
        #region collections

        public ObservableCollection<CostItemViewModel> CostItems => _RentViewModel.Costs;

        #endregion collections


        // commands
        #region commands

        public ICommand AddCostItemCommand { get; }


        public ICommand CloseCommand { get; }


        public ICommand DuplicateCostItemCommand { get; }


        public ICommand LeftPressCommand { get; }


        public ICommand RemoveCostItemCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public CostsViewModel(RentViewModel rentViewModel)
        {

            AddCostItemCommand = new RelayCommand((s) => AddCostItem(s), (s) => true);
            DuplicateCostItemCommand = new RelayCommand((s) => DuplicateCostItem(s), (s) => true);
            RemoveCostItemCommand = new RelayCommand((s) => RemoveCostItem(s), (s) => true);

            _RentViewModel = rentViewModel;

            _FlatViewModel = rentViewModel.GetFlatViewModel();


            if (_RentViewModel.CostsHasDataLock)
            {
                DataLockCheckbox = true;
            }

            CloseCommand = new RelayCommand((s) => Close(s), (s) => true);
            LeftPressCommand = new RelayCommand((s) => Drag(s), (s) => true);

            _RentViewModel.Costs.CollectionChanged += Costs_CollectionChanged;

            CalculateSum();

            RegisterCostItemValueChange();
        }

        #endregion constructors


        // methods
        #region methods

        private void AddCostItem(object s)
        {
            CostItemViewModel otherCostItemViewModel = new CostItemViewModel(new FinancialTransactionItem());

            _RentViewModel.AddCostItem(otherCostItemViewModel);
                       

            OnPropertyChanged(nameof(CostItems));
        }


        private void DuplicateCostItem(object s)
        {
            IList selection = (IList)s;

            if (selection != null)
            {
                var selected = selection.Cast<CostItemViewModel>().ToArray();

                CostItemViewModel costItemViewModel = selected.First();

                CostItemViewModel otherCostItemViewModel = new CostItemViewModel(new FinancialTransactionItem());

                otherCostItemViewModel.CostItem.TransactionItem = costItemViewModel.Item;
                otherCostItemViewModel.CostItem.TransactionShareTypes = costItemViewModel.CostShareTypes;
                otherCostItemViewModel.CostItem.TransactionSum = costItemViewModel.Cost;

                _RentViewModel.AddCostItem(otherCostItemViewModel);

                OnPropertyChanged(nameof(CostItems));
            }

                 
        }


        private void CalculateSum()
        {
            CostsSumPerMonth = 0.0;

            foreach (CostItemViewModel item in CostItems)
            {
                CostsSumPerMonth += item.Cost;
            }
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


        private void RegisterCostItemValueChange()
        {
            foreach (CostItemViewModel item in CostItems)
            {
                item.ValueChange += Item_ValueChange;
            }
        }



        private void RemoveCostItem(object s)
        {
            IList selection = (IList)s;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected other costs?",
                    "Remove Other TransactionSum(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<CostItemViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        _RentViewModel.RemoveCostItem(item);
                        OnPropertyChanged(nameof(CostItems));
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