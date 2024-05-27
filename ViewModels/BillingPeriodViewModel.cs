using SharedLivingCostCalculator.BoilerPlateReduction;
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    class BillingPeriodViewModel : BaseViewModel
    {
        private FlatViewModel flatViewModel;
        public bool BillingPeriodSelected { get; set; }

        private BillingPeriod _selectedValue; // private BillingPeriod _selectedBillingPeriod



        public BillingPeriod SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                BillingPeriodSelected = true;
                Rooms = CollectionViewSource.GetDefaultView(_selectedValue?.RoomConsumptionValues);
                Rooms?.Refresh();

                OnPropertyChanged(nameof(BillingPeriodSelected));
                OnPropertyChanged(nameof(SelectedValue));
                OnPropertyChanged(nameof(Rooms));
            }
        }

        public ICommand AddBillingPeriodCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICollectionView Billings { get; set; }
        public ICollectionView Rooms { get; set; }

        // room Count / Room class property

        public BillingPeriodViewModel(FlatViewModel _flatViewModel)
        { 
            flatViewModel = _flatViewModel;

            AddBillingPeriodCommand = new ActionCommand(p => AddBillingPeriod());
            DeleteCommand = new ActionCommand(p => DeleteBillingPeriod());

            Billings = CollectionViewSource.GetDefaultView(flatViewModel.BillingPeriods);
            Billings.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

            if (flatViewModel.BillingPeriods.Count == 0)
            {
                flatViewModel.BillingPeriods.Add(new Models.BillingPeriod(flatViewModel, Helper)
                {
                    StartDate = new DateTime(2024, 08, 02),
                    EndDate = new DateTime(2025, 08, 01),
                    TotalCostsPerPeriod = 1500,
                    TotalFixedCostsPerPeriod = 700,
                    TotalHeatingCostsPerPeriod = 800   
                });
                flatViewModel.BillingPeriods.Add(new Models.BillingPeriod(flatViewModel, Helper)
                {
                    StartDate = new DateTime(2023, 08, 02),
                    EndDate = new DateTime(2024, 08, 01),
                    TotalCostsPerPeriod = 1400,
                    TotalFixedCostsPerPeriod = 640,
                    TotalHeatingCostsPerPeriod = 760
                });

                flatViewModel.BillingPeriods.Add(new Models.BillingPeriod(flatViewModel, Helper)
                {
                    StartDate = new DateTime(2022, 08, 02),
                    EndDate = new DateTime(2023, 08, 01),
                    TotalCostsPerPeriod = 1450,
                    TotalFixedCostsPerPeriod = 720,
                    TotalHeatingCostsPerPeriod = 730
                });
            }
        }

        private void AddBillingPeriod()
        {
            BillingPeriod billingPeriod = new BillingPeriod(flatViewModel, Helper)
            {
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                EndDate = new DateTime(DateTime.Now.Year + 1, 1, 1)
            };

            flatViewModel.BillingPeriods.Add(billingPeriod);
            SelectedValue = billingPeriod;
        }

        private void DeleteBillingPeriod()
        {
            flatViewModel.BillingPeriods.Remove(SelectedValue);
            BillingPeriodSelected = false;
            OnPropertyChanged(nameof(BillingPeriodSelected));
        }

        public ValidationHelper Helper { get; } = new ValidationHelper();
    }
}
