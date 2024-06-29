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


        private readonly RentViewModel _RentViewModel;

        #endregion properties


        // collections
        #region collections

        public ObservableCollection<OtherCostItemViewModel> OtherCostItems => _RentViewModel.OtherCosts;

        #endregion collections


        // commands
        #region commands

        public ICommand AddOtherCostItemCommand { get; }


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


    }
}

// EOF