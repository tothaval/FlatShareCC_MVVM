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
using SharedLivingCostCalculator.Models;
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



        private RoomPaymentsViewModel _RoomPaymentsViewModel;
        public RoomPaymentsViewModel RoomPaymentsViewModel
        {
            get { return _RoomPaymentsViewModel; }
            set
            {
                _RoomPaymentsViewModel = value;
                OnPropertyChanged(nameof(RoomPaymentsViewModel));
            }
        }


        public RoomViewModel RoomViewModel => RoomPaymentsViewModel.RoomViewModel;


        public ObservableCollection<PaymentViewModel> Payments => GetPaymentViewModels();




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


        public PaymentsSetupViewModel(RoomPaymentsViewModel roomPaymentsViewModel)
        {
            _RoomPaymentsViewModel = roomPaymentsViewModel;
            _quantity = 1;

            AddPaymentCommand = new AddPaymentCommand(this);
            DeletePaymentCommand = new DeletePaymentCommand(this);

            _RoomPaymentsViewModel.RoomPayments.Payments.CollectionChanged += Payments_CollectionChanged;
        }


        private void Payments_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Payments));
            OnPropertyChanged(nameof(RoomPaymentsViewModel));
        }


        private ObservableCollection<PaymentViewModel> GetPaymentViewModels()
        {
            ObservableCollection<PaymentViewModel> paymentViewModels = new ObservableCollection<PaymentViewModel>();

            foreach (Payment payment in _RoomPaymentsViewModel.RoomPayments.Payments)
            {
                paymentViewModels.Add(new PaymentViewModel(payment));
            }

            return paymentViewModels;
        }


    }
}
// EOF