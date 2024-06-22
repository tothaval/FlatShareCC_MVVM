/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentManagementViewModel  : BaseViewModel
 * 
 *  viewmodel for RentManagementView
 *  
 *  displays the elements of ObservableCollection<RentViewModel>
 *  of the selected FlatViewModel instance, offers management functions
 *  to add or remove elements to the collection
 *  
 *  holds an instance of RentUpdateViewModel, which is responsible
 *  for editing the data of RentViewModel instances
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using SharedLivingCostCalculator.Views.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class RentManagementViewModel : BaseViewModel
    {
        private AccountingViewModel _accountingViewModel;


        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;


        public ICollectionView RentUpdates { get; }


        public bool RentUpdateSelected { get; set; }


        public bool HasRentUpdate => _flatViewModel.RentUpdates.Count > 0;


        private RentUpdateViewModel _updateViewModel;
        public RentUpdateViewModel UpdateViewModel
        {
            get { return _updateViewModel; }
            set
            {
                _updateViewModel = value;

                OnPropertyChanged(nameof(UpdateViewModel));
            }
        }


        private RentViewModel _selectedValue;
        public RentViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                UpdateViewModel = new RentUpdateViewModel(_flatViewModel, _selectedValue);

                RentUpdateSelected = true;
                OnPropertyChanged(nameof(RentUpdateSelected));
                OnPropertyChanged(nameof(SelectedValue));
            }
        }


        public ICommand AddRentUpdateCommand { get; }


        public ICommand DeleteCommand { get; }


        //private ObservableCollection<RentViewModel> _RentUpdates;
        //public ObservableCollection<RentViewModel> RentUpdates
        //{
        //    get { return _RentUpdates; }
        //    set
        //    {
        //        _RentUpdates = value;
        //        OnPropertyChanged(nameof(RentUpdates));
        //    }
        //}


        public ICollectionView Rents { get; set; }


        public RentManagementViewModel(AccountingViewModel accountingViewModel)
        {   
            _accountingViewModel = accountingViewModel;

            _flatViewModel = accountingViewModel.FlatViewModel;


            AddRentUpdateCommand = new RelayCommand(p => AddRentUpdate(), (s) => true);
            DeleteCommand = new RelayCommand(p => DeleteRentUpdate(), (s) => true);

            if (_flatViewModel != null)
            {
                if (_flatViewModel.RentUpdates.Count > 0)
                {
                    RentUpdates = CollectionViewSource.GetDefaultView(_flatViewModel.RentUpdates);
                    //RentUpdates.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

                    SelectedValue = _flatViewModel.GetMostRecentRent();

                    //OnPropertyChanged(nameof(RentUpdates));
                }
            }
        }

    
        private void AddRentUpdate()
        {
            RentViewModel rentViewModel = new RentViewModel(
                _flatViewModel,
                new Rent(_flatViewModel,
                    _flatViewModel.RentUpdates.Count,
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    0.0,
                    0.0,
                    0.0
                    )
                );

            _flatViewModel.RentUpdates.Add(rentViewModel);
            SelectedValue = rentViewModel;
            OnPropertyChanged(nameof(HasRentUpdate));
        }


        private void DeleteRentUpdate()
        {
            MessageBoxResult result = MessageBox.Show(
                $"Do you wan't to delete this rent data:\n\n" +
                $"\t{SelectedValue.GetFlatViewModel().Address}\n" +
                $"{SelectedValue.StartDate:d};\n{SelectedValue.ColdRent:C2};\n" +
                $"{SelectedValue.FixedCostsAdvance:C2};\n{SelectedValue.HeatingCostsAdvance:C2};\n",
                "Remove Rent", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _flatViewModel.RentUpdates.Remove(SelectedValue);
                RentUpdateSelected = false;
                OnPropertyChanged(nameof(RentUpdateSelected));
                OnPropertyChanged(nameof(HasRentUpdate));
                SelectedValue = null;

                if (_flatViewModel.RentUpdates.Count > 0)
                {
                    SelectedValue = _flatViewModel.RentUpdates[0];
                }
            }

        }


    }
}
// EOF