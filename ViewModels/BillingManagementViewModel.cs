using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class BillingManagementViewModel : BaseViewModel
    {
        private readonly AccountingViewModel _accountingViewModel;
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
        public ICommand ShowCostsCommand { get; }

        public ICollectionView Billings { get; set; }
        public ICollectionView Rooms { get; set; }


        public BillingManagementViewModel(FlatViewModel flatViewModel, AccountingViewModel accountingViewModel)
        {
            _accountingViewModel = accountingViewModel;
            _flatViewModel = flatViewModel;

            AddBillingPeriodCommand = new RelayCommand(p => AddBillingPeriod(), (s) => true);
            DeleteCommand = new RelayCommand(p => DeleteBillingPeriod(), (s) => true);

            ShowCostsCommand = new RelayCommand(p => ShowCosts(), (s) => true);

            Billings = CollectionViewSource.GetDefaultView(_flatViewModel.BillingPeriods);
            Billings.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

            if (_flatViewModel.BillingPeriods.Count > 0)
            {
                SelectedValue = _flatViewModel?.BillingPeriods?.First();
            }
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

        private void ShowCosts()
        {
            if (SelectedValue != null)
            {
                CostsView costs = new CostsView();
                costs.Owner = Application.Current.MainWindow;
                costs.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                costs.DataContext = new CostsViewModel(SelectedValue);

                // not sure yet on how to handle this, either a new rent is created automatically
                // once the show costs button is hit (check if it exists) or the user has to
                // create rents when needed and has to select a billing from a combobox in rentupdateview.

                //_flatViewModel.RentUpdates.Add(new RentViewModel(new Rent(), billingPeriod));

                costs.Show();
            }            
        }

        private void DeleteBillingPeriod()
        {
            _flatViewModel.BillingPeriods.Remove(SelectedValue);
            BillingPeriodSelected = false;
            OnPropertyChanged(nameof(BillingPeriodSelected));
        }
    }
}
