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
    internal class BillingManagementViewModel : BaseViewModel
    {
        private FlatViewModel _flatViewModel;
        public bool BillingPeriodSelected { get; set; }


        private BillingPeriodViewModel _updateViewModel;
        public BillingPeriodViewModel UpdateViewModel
        {
            get { return _updateViewModel; }
            set
            {
                _updateViewModel = value;
                OnPropertyChanged(nameof(UpdateViewModel));
            }
        }


        private BillingViewModel _selectedValue; // private Billing _selectedBillingPeriod

        public BillingViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                UpdateViewModel = new BillingPeriodViewModel(SelectedValue);

                BillingPeriodSelected = true;
                //Rooms = CollectionViewSource.GetDefaultView(_selectedValue?.RoomConsumptionValues);
                //Rooms?.Refresh();

                OnPropertyChanged(nameof(BillingPeriodSelected));
                OnPropertyChanged(nameof(SelectedValue));
                OnPropertyChanged(nameof(Rooms));
            }
        }

        public ICommand AddBillingPeriodCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICollectionView Billings { get; set; }
        public ICollectionView Rooms { get; set; }


        public BillingManagementViewModel(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;

            AddBillingPeriodCommand = new RelayCommand(p => AddBillingPeriod(), (s) => true);
            DeleteCommand = new RelayCommand(p => DeleteBillingPeriod(), (s) => true);

            Billings = CollectionViewSource.GetDefaultView(_flatViewModel.BillingPeriods);
            Billings.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));
        }

        private void AddBillingPeriod()
        {
            BillingViewModel billingPeriod = new BillingViewModel(new Billing(_flatViewModel)                
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year + 1, 1, 1)
            });

            _flatViewModel.BillingPeriods.Add(billingPeriod);
            SelectedValue = billingPeriod;

            OnPropertyChanged(nameof(SelectedValue));
        }

        private void DeleteBillingPeriod()
        {
            _flatViewModel.BillingPeriods.Remove(SelectedValue);
            BillingPeriodSelected = false;
            OnPropertyChanged(nameof(BillingPeriodSelected));
        }
    }
}
