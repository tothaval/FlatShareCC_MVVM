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
        public bool HasBillingPeriod => _flatViewModel.BillingPeriods.Count > 0;

        public string BillingManagementInstructionText { get; set; }

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

        private BillingViewModel _selectedValue;
        public BillingViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                UpdateViewModel = new BillingPeriodViewModel(_flatViewModel, SelectedValue);

                BillingPeriodSelected = true;

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
                OnPropertyChanged(nameof(HasBillingPeriod));
            }
        }

        private void AddBillingPeriod()
        {
            BillingViewModel billingPeriod = new BillingViewModel(_flatViewModel ,new Billing()                
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year + 1, 1, 1)
            });


            billingPeriod.GenerateRoomCosts();

            _flatViewModel.BillingPeriods.Add(billingPeriod);
            SelectedValue = billingPeriod;


            OnPropertyChanged(nameof(SelectedValue));
            OnPropertyChanged(nameof(HasBillingPeriod));
        }


        private void ShowCosts()
        {
            if (SelectedValue != null)
            {
                CostsView costs = new CostsView();
                costs.Owner = Application.Current.MainWindow;
                costs.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                costs.DataContext = new CostsViewModel(SelectedValue, _flatViewModel);

                costs.Show();
            }            
        }

        private void DeleteBillingPeriod()
        {
            MessageBoxResult result = MessageBox.Show(
                $"Do you wan't to delete this billing data:\n\n" +
                $"{SelectedValue.StartDate:d} - {SelectedValue.EndDate:d};\n" +
                $"{SelectedValue.TotalCostsPerPeriod:C2}\n" +
                $"{SelectedValue.TotalFixedCostsPerPeriod:C2}\n" +
                $"{SelectedValue.TotalHeatingCostsPerPeriod:C2}\n",
                "Remove Billing", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _flatViewModel.BillingPeriods.Remove(SelectedValue);
                BillingPeriodSelected = false;
                OnPropertyChanged(nameof(BillingPeriodSelected));
                OnPropertyChanged(nameof(HasBillingPeriod));
            }
        }
    }
}
