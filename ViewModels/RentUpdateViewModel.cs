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
        private FlatViewModel flatViewModel;
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


        public RentUpdateViewModel(FlatViewModel _flatViewModel)
        {
            flatViewModel = _flatViewModel;

            AddRentUpdateCommand = new ActionCommand(p => AddRentUpdate());
            DeleteCommand = new ActionCommand(p => DeleteRentUpdate());

            RentUpdates = CollectionViewSource.GetDefaultView(flatViewModel.RentUpdates);
            RentUpdates.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

            if (flatViewModel.RentUpdates.Count == 0)
            {
                flatViewModel.RentUpdates.Add(new Rent(new DateTime(2021, 9, 15), 500, 56, 89));
                flatViewModel.RentUpdates.Add(new Rent(new DateTime(2022, 10, 1), 520, 59, 94));
                flatViewModel.RentUpdates.Add(new Rent(new DateTime(2023, 10, 1), 550, 63, 97));
            }

        }

        private void AddRentUpdate()
        {
            Rent rent = new Rent();
            rent.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                 
            flatViewModel.RentUpdates.Add(rent);
            SelectedValue = rent;
        }
        private void DeleteRentUpdate()
        {
            flatViewModel.RentUpdates.Remove(SelectedValue);
            RentUpdateSelected = false;
            OnPropertyChanged(nameof(RentUpdateSelected));
        }
    }
}
