/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BillingManagementViewModel  : BaseViewModel
 * 
 *  viewmodel for BillingManagementView
 *  
 *  displays the elements of ObservableCollection<BillingViewModel>
 *  of the selected FlatViewModel instance, offers management functions
 *  to add or remove elements to the collection
 *  
 *  holds an instance of BillingPeriodViewModel, which is responsible
 *  for editing the data of BillingViewModel instances
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Views;
using System.ComponentModel;
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

            //AddBillingPeriodCommand = new RelayCommand(p => AddBillingPeriod(), (s) => true);
            //DeleteCommand = new RelayCommand(p => DeleteBillingPeriod(), (s) => true);

            ShowCostsCommand = new RelayCommand(p => ShowCosts(), (s) => true);

            Billings = CollectionViewSource.GetDefaultView(_flatViewModel.BillingPeriods);
            Billings.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

            if (_flatViewModel.BillingPeriods.Count > 0
                && _accountingViewModel.Rents != null
                && _accountingViewModel.Rents.SelectedValue != null
                && _accountingViewModel.Rents.SelectedValue.HasBilling == true)
            {
                SelectedValue = _accountingViewModel.Rents.SelectedValue.BillingViewModel;
                OnPropertyChanged(nameof(HasBillingPeriod));
            }
            else if (_flatViewModel.BillingPeriods.Count > 0)
            {
                SelectedValue = _flatViewModel.BillingPeriods.First();
            }
        }


        private void RentManagementViewModelSelectionChange(object? sender, EventArgs e)
        {
            BillingViewModel? billingViewModel = _accountingViewModel.Rents.SelectedValue.BillingViewModel;

            if (billingViewModel != null)
            {
                SelectedValue = billingViewModel;
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
// EOF