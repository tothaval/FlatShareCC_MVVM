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
    internal class RentManagementViewModel : BaseViewModel
    {
        private FlatViewModel _flatViewModel;


        public bool RentUpdateSelected { get; set; }
        public bool HasRentUpdate => _flatViewModel.RentUpdates.Count > 0;

        public string RentManagementInstructionText { get; set; }

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

                UpdateViewModel = new RentUpdateViewModel(_flatViewModel, SelectedValue);

                RentUpdateSelected = true;
                OnPropertyChanged(nameof(RentUpdateSelected));
                OnPropertyChanged(nameof(SelectedValue));
            }
        }

        public ICommand AddRentUpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ShowCostsCommand { get; }

        public ICollectionView RentUpdates { get; set; }



        public RentManagementViewModel(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;

            AddRentUpdateCommand = new RelayCommand(p => AddRentUpdate(), (s) => true);
            DeleteCommand = new RelayCommand(p => DeleteRentUpdate(), (s) => true);

            ShowCostsCommand = new RelayCommand(p => ShowCosts(), (s) => true);

            RentUpdateSelected = false;

            RentUpdates = CollectionViewSource.GetDefaultView(this._flatViewModel.RentUpdates);
            RentUpdates.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

            if (_flatViewModel.RentUpdates.Count > 0)
            {
                SelectedValue = _flatViewModel.GetMostRecentRent();                    
            }
        }


        private void AddRentUpdate()
        {
            RentViewModel rentViewModel = new RentViewModel(
                _flatViewModel,
                new Rent(_flatViewModel.RentUpdates.Count,
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
            }

        }


        private void ShowCosts()
        {
            // if objects are chained together (f.e. a rent object and a billing object)
            // they should be selected together in every relevant tab

            // calculate costs button on RentUpdates as well? or make it so that
            // rent is automatically calculated per area share, until/unless a
            // combobox item is selected, which must be a billing.            
            // CostsViewModel should be able to handle both types of viewmodels
            // it has to check if a billing view model is present, oth


            if (SelectedValue != null)
            {
                CostsView costs = new CostsView();
                costs.Owner = Application.Current.MainWindow;
                costs.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                costs.DataContext = new CostsViewModel(SelectedValue, _flatViewModel);


                costs.Show();
            }
        }
    }
}
