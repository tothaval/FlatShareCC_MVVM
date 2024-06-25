/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomPaymentsViewModel  : BaseViewModel
 * 
 *  viewmodel for RoomPayments model
 *  
 *  purpose:
 *      -> calculate payments for a room instance
 *          within IRoomCostCarrier class BillingViewModel
 */
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class RoomPaymentsViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        // properties & fields
        #region properties & fields

        public double CombinedPayments => CalculateTotalPayments();


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public double GetRoomArea()
        {
            if (RoomPayments != null && RoomPayments.RoomViewModel != null)
            {
                return RoomPayments.RoomViewModel.RoomArea;
            }

            return 0.0;
        }


        public string GetRoomName()
        {
            if (RoomPayments != null && RoomPayments.RoomViewModel != null)
            {
                return RoomPayments.RoomViewModel.RoomName;
            }

            return "unknown";
        }


        public bool HasErrors => _helper.HasErrors;


        private ValidationHelper _helper = new ValidationHelper();


        public int ID => RoomPayments.RoomID;


        public double RoomArea => GetRoomArea();


        public string RoomName => GetRoomName();



        private RoomPayments _RoomPayments;
        public RoomPayments RoomPayments
        {
            get { return _RoomPayments; }
            set
            {
                _RoomPayments = value;

                OnPropertyChanged(nameof(RoomPayments));
            }
        }

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion event properties & fields


        // collections
        #region collections

        public ObservableCollection<PaymentViewModel> PaymentViewModels => GetPaymentViewModels();

        #endregion collections


        // constructors
        #region constructors   

        public RoomPaymentsViewModel(RoomPayments roomPayments)
        {
            RoomPayments = roomPayments;
        }
        
        #endregion constructors


        // methods
        #region methods

        public double CalculateTotalPayments()
        {
            double total = 0.0;

            if (RoomPayments != null)
            {
                foreach (Payment payment in RoomPayments.Payments)
                {
                    total += payment.PaymentTotal;
                }
            }

            return total;
        }


        private ObservableCollection<PaymentViewModel> GetPaymentViewModels()
        {
            ObservableCollection<PaymentViewModel> paymentViewModels = new ObservableCollection<PaymentViewModel>();

            foreach (Payment payment in RoomPayments.Payments)
            {
                PaymentViewModel paymentViewModel = new PaymentViewModel(payment);

                paymentViewModel.PaymentChange += PaymentViewModel_PaymentChange;

                paymentViewModels.Add(paymentViewModel);
            }

            return paymentViewModels;
        }

        #endregion methods


        // events
        #region events

        private void PaymentViewModel_PaymentChange(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CombinedPayments));
        }

        #endregion events


    }
}
// EOF