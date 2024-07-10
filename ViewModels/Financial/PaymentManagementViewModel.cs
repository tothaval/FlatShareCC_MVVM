/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;


namespace SharedLivingCostCalculator.ViewModels.Financial
{
    internal class PaymentManagementViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        // properties & fields
        #region properties

        private readonly BillingViewModel _billingViewModel;


        private readonly FlatViewModel _flatViewModel;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public bool HasErrors => _helper.HasErrors;


        private ValidationHelper _helper = new ValidationHelper();


        public ICollectionView RoomPayments { get; set; }


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

        #endregion properties


        // event properties & fields
        #region event properties

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion event properties


        // constructors
        #region constructors

        public PaymentManagementViewModel(BillingViewModel billingViewModel)
        {
            _flatViewModel = billingViewModel.GetFlatViewModel();
            _billingViewModel = billingViewModel;

            RoomPayments = CollectionViewSource.GetDefaultView(_billingViewModel.RoomPayments);
            RoomPayments.SortDescriptions.Add(new SortDescription("RoomViewModel.ID", ListSortDirection.Ascending));

            _billingViewModel.RoomPayments.CollectionChanged += RoomPayments_CollectionChanged;

        }

        #endregion constructors


        // events
        #region events

        private void RoomPayments_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RoomPayments));
            OnPropertyChanged(nameof(UpdateViewModel));
            OnPropertyChanged(nameof(SelectedValue));
        }

        #endregion events


    }
}
// EOF