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

            if (_flatViewModel.BillingPeriods.Count == 0)
            {
                _flatViewModel.BillingPeriods.Add(new BillingViewModel(new Models.Billing(_flatViewModel)
                {
                    StartDate = new DateTime(2024, 08, 02),
                    EndDate = new DateTime(2025, 08, 01),
                    TotalCostsPerPeriod = 1500,
                    TotalFixedCostsPerPeriod = 700,
                    TotalHeatingCostsPerPeriod = 800,
                    TotalHeatingUnitsConsumption = 5672
                }));
                _flatViewModel.BillingPeriods.Add(new BillingViewModel(new Models.Billing(_flatViewModel)
                {
                    StartDate = new DateTime(2023, 08, 02),
                    EndDate = new DateTime(2024, 08, 01),
                    TotalCostsPerPeriod = 1400,
                    TotalFixedCostsPerPeriod = 640,
                    TotalHeatingCostsPerPeriod = 760,

                    TotalHeatingUnitsConsumption = 2223,
                    TotalHeatingUnitsRoom = 1675
                }));

                _flatViewModel.BillingPeriods.Add(new BillingViewModel(new Models.Billing(_flatViewModel)
                {
                    StartDate = new DateTime(2022, 08, 02),
                    EndDate = new DateTime(2023, 08, 01),
                    TotalCostsPerPeriod = 1450,
                    TotalFixedCostsPerPeriod = 720,
                    TotalHeatingCostsPerPeriod = 730,
                    TotalHeatingUnitsConsumption = 1400,
                    TotalHeatingUnitsRoom = 1238
                }));

                int iterator = 15;

                foreach (RoomHeatingUnits item in _flatViewModel.BillingPeriods[0].RoomConsumptionValues)
                {
                    item.HeatingUnitsConsumption = 1115 + iterator;
                    iterator += 15;

                    _flatViewModel.BillingPeriods[0].TotalHeatingUnitsRoom += item.HeatingUnitsConsumption;
                }

                //_flatViewModel.BillingPeriods[0].CalculateRoomsConsumption();

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

        private void DeleteBillingPeriod()
        {
            _flatViewModel.BillingPeriods.Remove(SelectedValue);
            BillingPeriodSelected = false;
            OnPropertyChanged(nameof(BillingPeriodSelected));
        }
    }
}
