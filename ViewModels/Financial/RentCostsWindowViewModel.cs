/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
    public class RentCostsWindowViewModel : BaseViewModel
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


        public RentViewModel RentViewModel { get; }


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

        public RentCostsWindowViewModel(RentViewModel rentViewModel)
        {

            AddFinacialTransactionItemCommand = new RelayCommand((s) => AddFinacialTransactionItem(s), (s) => true);
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
            FinancialTransactionItemRentViewModel otherCostItemViewModel = new FinancialTransactionItemRentViewModel(new FinancialTransactionItemRent());

            //otherCostItemViewModel.ValueChange += Item_ValueChange;

            RentViewModel.AddFinacialTransactionItem(otherCostItemViewModel);

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
                    var selected = selection.Cast<FinancialTransactionItemRentViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        RentViewModel.RemoveFinancialTransactionItemViewModel(item);

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