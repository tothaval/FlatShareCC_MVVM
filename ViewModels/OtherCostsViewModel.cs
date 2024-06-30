/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *   
 *  OtherCostsViewModel  : BaseViewModel
 * 
 *  viewmodel for OtherCostsView
 *  
 *  displays a seperate window for creating
 *  or editing of OtherCostItemViewModel instances
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    public class OtherCostsViewModel : BaseViewModel
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
                _RentViewModel.OtherCostsHasDataLock = value;
                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(DataLock));
            }
        }


        public bool DataLock => !DataLockCheckbox;


        private readonly FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        private double _OtherCostsSumPerMonth;
        public double OtherCostsSumPerMonth
        {
            get { return _OtherCostsSumPerMonth; }
            set
            {
                _OtherCostsSumPerMonth = value;
                OnPropertyChanged(nameof(OtherCostsSumPerMonth));
            }
        }


        private readonly RentViewModel _RentViewModel;

        #endregion properties


        // collections
        #region collections

        public ObservableCollection<OtherCostItemViewModel> OtherCostItems => _RentViewModel.OtherCosts;

        #endregion collections


        // commands
        #region commands

        public ICommand AddOtherCostItemCommand { get; }


        public ICommand CloseCommand { get; }


        public ICommand LeftPressCommand { get; }


        public ICommand RemoveOtherCostItemCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public OtherCostsViewModel(RentViewModel rentViewModel)
        {

            AddOtherCostItemCommand = new RelayCommand((s) => AddOtherCostItem(s), (s) => true);
            RemoveOtherCostItemCommand = new RelayCommand((s) => RemoveOtherCostItem(s), (s) => true);

            _RentViewModel = rentViewModel;

            _FlatViewModel = rentViewModel.GetFlatViewModel();


            if (_RentViewModel.OtherCostsHasDataLock)
            {
                DataLockCheckbox = true;
            }

            CloseCommand = new RelayCommand((s) => Close(s), (s) => true);
            LeftPressCommand = new RelayCommand((s) => Drag(s), (s) => true);

            _RentViewModel.OtherCosts.CollectionChanged += OtherCosts_CollectionChanged;

            CalculateSum();

            RegisterOtherCostItemValueChange();
        }

        #endregion constructors


        // methods
        #region methods

        private void AddOtherCostItem(object s)
        {
            OtherCostItemViewModel otherCostItemViewModel = new OtherCostItemViewModel(new OtherCostItem());

            _RentViewModel.OtherCosts.Add(otherCostItemViewModel);

            OnPropertyChanged(nameof(OtherCostItems));
        }


        private void CalculateSum()
        {
            OtherCostsSumPerMonth = 0.0;

            foreach (OtherCostItemViewModel item in OtherCostItems)
            {
                OtherCostsSumPerMonth += item.Cost;
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


        private void RegisterOtherCostItemValueChange()
        {
            foreach (OtherCostItemViewModel item in OtherCostItems)
            {
                item.ValueChange += Item_ValueChange;
            }
        }


        private void Item_ValueChange(object? sender, EventArgs e)
        {
            CalculateSum();
        }


        private void RemoveOtherCostItem(object s)
        {
            IList selection = (IList)s;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected other costs?",
                    "Remove Other Cost(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<OtherCostItemViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        _RentViewModel.OtherCosts.Remove(item);
                        OnPropertyChanged(nameof(OtherCostItems));
                    }
                }
            }
        }

        #endregion methods


        // events
        #region events

        private void OtherCosts_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RegisterOtherCostItemValueChange();

            CalculateSum();
        }

        #endregion events


    }
}

// EOF