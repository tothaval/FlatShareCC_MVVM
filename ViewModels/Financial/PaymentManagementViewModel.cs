/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
using System.ComponentModel;
using System.Windows.Data;


namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class PaymentManagementViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        // properties & fields
        #region properties

        private readonly BillingViewModel _BillingViewModel;


        private readonly FlatViewModel _FlatViewModel;


        public IEnumerable GetErrors(string? propertyName) => _Helper.GetErrors(propertyName);


        public bool HasErrors => _Helper.HasErrors;


        private ValidationHelper _Helper = new ValidationHelper();


        public ICollectionView RoomPayments { get; set; }


        private RoomPaymentsViewModel _SelectedValue; // private Billing _selectedBillingPeriod
        public RoomPaymentsViewModel SelectedValue
        {
            get { return _SelectedValue; }
            set
            {
                if (_SelectedValue == value) return;
                _SelectedValue = value;

                UpdateViewModel = new PaymentsSetupViewModel(SelectedValue, _BillingViewModel);

                OnPropertyChanged(nameof(SelectedValue));
            }
        }


        private PaymentsSetupViewModel _UpdateViewModel;
        public PaymentsSetupViewModel UpdateViewModel
        {
            get { return _UpdateViewModel; }
            set
            {
                _UpdateViewModel = value;
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
            _FlatViewModel = billingViewModel.GetFlatViewModel();
            _BillingViewModel = billingViewModel;

            RoomPayments = CollectionViewSource.GetDefaultView(_BillingViewModel.RoomPayments);
            RoomPayments.SortDescriptions.Add(new SortDescription("RoomViewModel.ID", ListSortDirection.Ascending));

            _BillingViewModel.RoomPayments.CollectionChanged += RoomPayments_CollectionChanged;

            OnPropertyChanged(nameof(RoomPayments));
            OnPropertyChanged(nameof(UpdateViewModel));
            OnPropertyChanged(nameof(SelectedValue));
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