using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class RentManagementViewModel : BaseViewModel
    {
        private FlatViewModel _flatViewModel;
        public bool RentUpdateSelected { get; set; }


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


        private RentViewModel _selectedValue; // private Billing _selectedBillingPeriod
        public RentViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                UpdateViewModel = new RentUpdateViewModel(SelectedValue);

                RentUpdateSelected = true;
                OnPropertyChanged(nameof(RentUpdateSelected));
                OnPropertyChanged(nameof(SelectedValue));
            }
        }

        public ICommand AddRentUpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICollectionView RentUpdates { get; set; }



        public RentManagementViewModel(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;

            AddRentUpdateCommand = new RelayCommand(p => AddRentUpdate(), (s) => true);
            DeleteCommand = new RelayCommand(p => DeleteRentUpdate(), (s) => true);

            RentUpdateSelected = false;

            RentUpdates = CollectionViewSource.GetDefaultView(this._flatViewModel.RentUpdates);
            RentUpdates.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

            if (this._flatViewModel.RentUpdates.Count == 0)
            {
                _flatViewModel.RentUpdates.Add(new RentViewModel(new Rent(
                    new DateTime(2021, 9, 15), 500, 56, 89))
                    );
                _flatViewModel.RentUpdates.Add(new RentViewModel(new Rent(new DateTime(2022, 10, 1), 520, 59, 94)));
                _flatViewModel.RentUpdates.Add(new RentViewModel(new Rent(new DateTime(2023, 10, 1), 550, 63, 97)));
            }
        }


        private void AddRentUpdate()
        {
            RentViewModel rentViewModel = new RentViewModel(
                new Rent(
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    0.0,
                    0.0,
                    0.0
                    )
                );

            _flatViewModel.RentUpdates.Add(rentViewModel);
            SelectedValue = rentViewModel;
        }
        private void DeleteRentUpdate()
        {
            _flatViewModel.RentUpdates.Remove(SelectedValue);
            RentUpdateSelected = false;
            OnPropertyChanged(nameof(RentUpdateSelected));
        }
    }
}
