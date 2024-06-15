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
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;


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


        public ObservableCollection<RoomViewModel> Rooms => _flatViewModel.Rooms;


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


        private RoomViewModel _selectedValue; // private Billing _selectedBillingPeriod
        public RoomViewModel SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                if (_selectedValue == value) return;
                _selectedValue = value;

                UpdateViewModel = new PaymentsSetupViewModel(SelectedValue);

                OnPropertyChanged(nameof(SelectedValue));

                foreach (RoomViewModel room in Rooms)
                {
                    room.RegisterPaymentEvents();
                    room.DetermineValues();
                }
            }
        }


        public PaymentManagementViewModel(FlatViewModel flatViewModel)
        {
            _flatViewModel = flatViewModel;

            foreach (RoomViewModel room in Rooms)
            {
                room.DetermineValues();
            }
        }


    }
}
// EOF