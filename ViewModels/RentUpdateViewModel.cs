using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    class RentUpdateViewModel : BaseViewModel
    {
        private FlatViewModel _flatViewModel;
        public bool RentUpdateSelected { get; set; }

        private Rent _selectedValue; // private BillingPeriod _selectedBillingPeriod

        public Rent SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                RentUpdateSelected = true;
                OnPropertyChanged(nameof(RentUpdateSelected));
                OnPropertyChanged(nameof(SelectedValue));
            }
        }

        public ICommand AddRentUpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICollectionView RentUpdates { get; set; }


        public RentUpdateViewModel(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;

            AddRentUpdateCommand = new RelayCommand(p => AddRentUpdate(), (s) => true);
            DeleteCommand = new RelayCommand(p => DeleteRentUpdate(), (s) => true);

            RentUpdates = CollectionViewSource.GetDefaultView(this._flatViewModel.RentUpdates);
            RentUpdates.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

            if (this._flatViewModel.RentUpdates.Count == 0)
            {
                _flatViewModel.RentUpdates.Add(new Rent(new DateTime(2021, 9, 15), 500, 56, 89, _flatViewModel));
                _flatViewModel.RentUpdates.Add(new Rent(new DateTime(2022, 10, 1), 520, 59, 94, _flatViewModel));
                _flatViewModel.RentUpdates.Add(new Rent(new DateTime(2023, 10, 1), 550, 63, 97, _flatViewModel));
            }
        }

        private void AddRentUpdate()
        {
            Rent rent = new Rent(_flatViewModel);
            rent.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                 
            _flatViewModel.RentUpdates.Add(rent);
            SelectedValue = rent;
        }
        private void DeleteRentUpdate()
        {
            _flatViewModel.RentUpdates.Remove(SelectedValue);
            RentUpdateSelected = false;
            OnPropertyChanged(nameof(RentUpdateSelected));
        }
    }
}
