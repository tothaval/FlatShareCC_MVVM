/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PaymentManagementViewModel  : BaseViewModel
 * 
 *  viewmodel for PaymentManagementView
 *  
 *  displays ObservableCollection<RoomViewModel> elements
 *  and shows a PaymentSetupViewModel for a selected
 *  instance of RoomViewModel
 */
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;


namespace SharedLivingCostCalculator.ViewModels
{
    internal class PaymentManagementViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        private ValidationHelper _helper = new ValidationHelper();


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;


        public bool HasErrors => _helper.HasErrors;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        private readonly FlatViewModel _flatViewModel;
        private readonly BillingViewModel _billingViewModel;


        private PaymentsSetupViewModel _updateViewModel;
        public PaymentsSetupViewModel UpdateViewModel
        {
            get { return _updateViewModel; }
            set
            {
                _updateViewModel = value;
                OnPropertyChanged(nameof(UpdateViewModel));
            }
        }


        private RoomPaymentsViewModel _selectedValue; // private Billing _selectedBillingPeriod
        public RoomPaymentsViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                UpdateViewModel = new PaymentsSetupViewModel(SelectedValue);

                OnPropertyChanged(nameof(SelectedValue));
            }
        }


        public ICollectionView RoomPayments { get; set; }

        public PaymentManagementViewModel(BillingViewModel billingViewModel)
        {
            _flatViewModel = billingViewModel.GetFlatViewModel();
            _billingViewModel = billingViewModel;

            RoomPayments = CollectionViewSource.GetDefaultView(_billingViewModel.RoomPayments);
            RoomPayments.SortDescriptions.Add(new SortDescription("RoomViewModel.ID", ListSortDirection.Ascending));

            _billingViewModel.RoomPayments.CollectionChanged += RoomPayments_CollectionChanged;
            
        }

        private void RoomPayments_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RoomPayments));
            OnPropertyChanged(nameof(UpdateViewModel));
            OnPropertyChanged(nameof(SelectedValue));
        }
    }
}
// EOF