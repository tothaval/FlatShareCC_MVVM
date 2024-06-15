/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PaymentsSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for PaymentsSetupView
 *  
 *  displays all elements of ObservableCollection<PaymentViewModel>
 *  for a selected instance of RoomViewModel
 *  
 *  create, edit or delete instances of PaymentViewModel for a
 *  selected RoomViewModel instance
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Utility;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class PaymentsSetupViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        private ValidationHelper _helper = new ValidationHelper();


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;


        public bool HasErrors => _helper.HasErrors;

        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        private readonly RoomViewModel _roomViewModel;
        public RoomViewModel RoomViewModel => _roomViewModel;


        public ObservableCollection<PaymentViewModel> Payments => _roomViewModel.Payments;


        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _helper.ClearError(nameof(Quantity));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(Quantity));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(Quantity));
                }
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }


        public ICommand AddPaymentCommand { get; }


        public ICommand DeletePaymentCommand { get; }


        public PaymentsSetupViewModel(RoomViewModel roomViewModel)
        {
            _roomViewModel = roomViewModel;
            _quantity = 1;

            AddPaymentCommand = new AddPaymentCommand(this);
            DeletePaymentCommand = new DeletePaymentCommand(this);
        }


    }
}
// EOF